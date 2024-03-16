using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Chat;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Internship;
using RecruitmentSystem.Domain.Dtos.OpenAi;

namespace RecruitmentSystem.Business.Services;

public class OpenAiService
{
    private IConfiguration _configuration;
    private RecruitmentDbContext _db;
    private PdfService _pdfService;
    private IMapper _mapper;
    
    public OpenAiService(IConfiguration configuration, RecruitmentDbContext db,
        PdfService pdfService, IMapper mapper)
    {
        _configuration = configuration;
        _db = db;
        _pdfService = pdfService;
        _mapper = mapper;
    }

    private async Task<string> GenerateScreeningPrompt(Guid applicaitonId)
    {
        var cv = await _db.Cvs.FirstOrDefaultAsync(cv => cv.ApplicationId.Equals(applicaitonId));

        var cvContent = _pdfService.GetTextFromPdf(cv.FileContent);

        var screeningPrompt = new ScreeningPrompt
        {
            CvContent = cvContent,
            InternshipDescription = await GenerateScreeningPromptDescription(applicaitonId)
        };
        
        var dataStringJson = JsonConvert.SerializeObject(screeningPrompt);

        return dataStringJson;
    }

    private async Task<string> GenerateScreeningPromptDescription(Guid applicationId)
    {
        var application = await _db.Applications
            .FirstOrDefaultAsync(a => a.Id.Equals(applicationId));

        var internship = await _db.Internships
            .FirstOrDefaultAsync(i => i.Id.Equals(application.InternshipId));

        var dto = _mapper.Map<InternshipForScreeningPromptDto>(internship);

        var descriptionStringJson = JsonConvert.SerializeObject(dto);

        return descriptionStringJson;
    }

    public async Task<ScreeningScoreResponse?> GetScreeningScore(Guid applicationId)
    {
        var api = new OpenAIAPI(_configuration["OPENAI_SECRET"]);

        var prompt = await GenerateScreeningPrompt(applicationId);

        var chatRequest = new ChatRequest
        {
            Model = "gpt-3.5-turbo-1106",
            Temperature = 1,
            MaxTokens = 256,
            ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
            TopP = 1,
            Messages = new[] {
                new ChatMessage(ChatMessageRole.System, "You're an assistant in resume grading tasked with comparing how well the USER's CV matches a" +
                                                        " given internship description. You will receive internship details for which you'll compare the user's " +
                                                        "CV. Your task is to assess the fitness score, which indicates how well-suited the internship and the " +
                                                        "resume are for each other. The fitness score ranges from 1 (indicating a poor fit) to 5" +
                                                        " (indicating the best fit). Provide output in valid JSON. The JSON should be in this format:" +
                                                        " {\"fitnessScore\": {score}"),
                new ChatMessage(ChatMessageRole.User, prompt)
            }
        };
        
        var result = await api.Chat.CreateChatCompletionAsync(chatRequest);
        
        return JsonConvert.DeserializeObject<ScreeningScoreResponse>(result.ToString());;
    }
}