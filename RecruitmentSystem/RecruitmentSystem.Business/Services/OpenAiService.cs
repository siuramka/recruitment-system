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

    private async Task<string> GenerateInterviewPrompt(Guid interviewId)
    {
        var interview = await _db.Interviews
            .FirstOrDefaultAsync(c => c.Id == interviewId);

        var evaluation = await _db.Evaluations.FirstOrDefaultAsync(e => e.Id == interview.EvaluationId);

        var interviewScorePrompt = new InterviewScorePrompt
        {
            InterviewReviewContent = evaluation.Content,
            InterviewCompanyScore = evaluation.CompanyScore,
            InternshipDescription = await GenerateScreeningPromptDescription(interview.ApplicationId)
        };

        var dataStringJson = JsonConvert.SerializeObject(interviewScorePrompt);
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

    private async Task<string> GenerateAssesmentPrompt(Guid assessmentId)
    {
        var assessment = await _db.Assessments
            .FirstOrDefaultAsync(c => c.Id == assessmentId);

        var evaluation = await _db.Evaluations.FirstOrDefaultAsync(e => e.Id == assessment.EvaluationId);

        var assessmentScorePrompt = new AssessmentScorePrompt
        {
            AssessmentReviewContent = evaluation.Content,
            AssessmentCompanyScore = evaluation.CompanyScore,
            InternshipDescription = await GenerateScreeningPromptDescription(assessment.ApplicationId)
        };

        var dataStringJson = JsonConvert.SerializeObject(assessmentScorePrompt);
        return dataStringJson;
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
            Messages = new[]
            {
                new ChatMessage(ChatMessageRole.System,
                    "You're an AI expert tasked in resume grading tasked with comparing how well the USER's CV matches a" +
                    " given internship description. You will receive internship details for which you'll compare the user's " +
                    "CV. Your task is to assess the fitness score, which indicates how well-suited the internship and the " +
                    "resume are for each other. The fitness score ranges from 1 (indicating a poor fit) to 5" +
                    " (indicating the best fit). Provide output in valid JSON. The JSON should be in this format:" +
                    " {\"fitnessScore\": {score}"),
                new ChatMessage(ChatMessageRole.User, prompt)
            }
        };

        var result = await api.Chat.CreateChatCompletionAsync(chatRequest);

        return JsonConvert.DeserializeObject<ScreeningScoreResponse>(result.ToString());
        ;
    }

    public async Task<InterviewScoreResponse?> GetInterviewScore(Guid interviewId)
    {
        var api = new OpenAIAPI(_configuration["OPENAI_SECRET"]);

        var prompt = await GenerateInterviewPrompt(interviewId);

        var chatRequest = new ChatRequest
        {
            Model = "gpt-3.5-turbo-1106",
            Temperature = 1,
            MaxTokens = 256,
            ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
            TopP = 1,
            Messages = new[]
            {
                new ChatMessage(ChatMessageRole.System,
                    "You're an AI expert tasked with grading interview performance for a internship candidacy. You will receive internship details, You will receive the date " +
                    "of the interview: the score given by the recruiter: 1 (indicating excellent performance in the interview) to 5 (indicating excellent performance in the interview) ," +
                    " and any additional notes/review provided by the recruitet. Your task is to evaluate the " +
                    "interview and assign an interview score. The interview score should be in the range of 1 (indicating poor " +
                    "performance) to 5 (indicating excellent performance). You must take into account everything in the recruiter notes/review, " +
                    "and how fit this person would be for the internship description. Provide output in valid JSON format. The JSON should be in " +
                    "this format: {\"interviewScore\": {score}}."),
                new ChatMessage(ChatMessageRole.User, prompt)
            }
        };

        var result = await api.Chat.CreateChatCompletionAsync(chatRequest);

        return JsonConvert.DeserializeObject<InterviewScoreResponse>(result.ToString());
        ;
    }

    public async Task<AssessmentScoreResponse?> GetAssessmentScore(Guid assessmentId)
    {
        var api = new OpenAIAPI(_configuration["OPENAI_SECRET"]);

        var prompt = await GenerateAssesmentPrompt(assessmentId);

        var chatRequest = new ChatRequest
        {
            Model = "gpt-3.5-turbo-1106",
            Temperature = 1,
            MaxTokens = 256,
            ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
            TopP = 1,
            Messages = new[]
            {
                new ChatMessage(ChatMessageRole.System,
                    "You're an AI expert tasked with grading interview performance for a internship candidacy. You will receive internship details, You're " +
                    "an AI assistant tasked with assessing a candidate's performance on a given set of tasks for an internship candidacy. You will receive details" +
                    " about the tasks assigned to the candidate and the recruiter's review of their performance. Your task is to evaluate the candidate's " +
                    "performance based on the provided review and task details and assign an assessment score. The assessment score should be in the range of 1 " +
                    "(indicating poor performance) to 5 (indicating excellent performance). You must consider everything in the recruiter's review and how well " +
                    "the candidate fulfilled the requirements of the assigned tasks. Provide output in valid JSON format. The JSON should be in this format: " +
                    "{\"assessmentScore\": {score}}."),
                new ChatMessage(ChatMessageRole.User, prompt)
            }
        };

        var result = await api.Chat.CreateChatCompletionAsync(chatRequest);

        return JsonConvert.DeserializeObject<AssessmentScoreResponse>(result.ToString());
    }
}