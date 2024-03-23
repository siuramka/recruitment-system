using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Chat;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Decision;
using RecruitmentSystem.Domain.Dtos.Internship;
using RecruitmentSystem.Domain.Dtos.OpenAi;
using RecruitmentSystem.Domain.Models;

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

    private async Task<List<Evaluation>> GetApplicationEvaluations(Guid applicationId)
    {
        return await _db.Evaluations
            .Include(e => e.Cv)
            .Include(e => e.Assessment)
            .Include(e => e.Interview)
            .Where(e => e.ApplicationId.Equals(applicationId))
            .ToListAsync();
    }

    private static async Task<string> GetEvaluationStepName(Evaluation evaluation)
    {
        if (evaluation.Cv != null)
        {
            return StepType.Screening.ToString();
        }

        if (evaluation.Assessment != null)
        {
            return StepType.Assessment.ToString();
        }

        if (evaluation.Interview != null)
        {
            return StepType.Interview.ToString();
        }

        return "";
    }

    private async Task<List<StepEvaluation>> GetStepEvaluations(Guid applicationId)
    {
        var evaluations = await GetApplicationEvaluations(applicationId);

        var stepEvaluations = new List<StepEvaluation>();

        foreach (var evaluation in evaluations)
        {
            var stepEvaluation = new StepEvaluation
            {
                StepName = await GetEvaluationStepName(evaluation),
                AiScoreForCandidateInStep = evaluation.AiScore,
                CompanyScoreForCandidateInStep = evaluation.CompanyScore,
                CompanysReviewOfCandidateInStep = evaluation.Content
            };

            stepEvaluations.Add(stepEvaluation);
        }

        return stepEvaluations;
    }

    private async Task<string> GenerateDecisionPrompt(Guid applicationId)
    {
        var internshipDescription = await GenerateScreeningPromptDescription(applicationId);
        var stepEvaluations = JsonConvert.SerializeObject(await GetStepEvaluations(applicationId));

        var companyReview = await _db.Decisions.FirstOrDefaultAsync(x => x.ApplicationId == applicationId);

        var decisionPrompt = new DecisionPrompt
        {
            InternshipDescription = internshipDescription,
            StepEvaluations = stepEvaluations,
            FinalCompanyReviewOfCandidate = companyReview.CompanySummary,
        };

        var dataStringJson = JsonConvert.SerializeObject(decisionPrompt);
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
                    "CV. The data provided to you, will be in JSON format.  Your task is to assess the fitness score, which indicates how well-suited the internship and the " +
                    "resume are for each other. The fitness score ranges from 1 (indicating a poor fit) to 5" +
                    " (indicating the best fit). Provide output in valid JSON. The JSON should be in this format:" +
                    " {\"fitnessScore\": {score}"),
                new ChatMessage(ChatMessageRole.User, prompt)
            }
        };

        var result = await api.Chat.CreateChatCompletionAsync(chatRequest);

        return JsonConvert.DeserializeObject<ScreeningScoreResponse>(result.ToString());
    }

    public async Task<DecisionResponse?> GetFinalDecision(Guid applicationId)
    {
        var api = new OpenAIAPI(_configuration["OPENAI_SECRET"]);

        var prompt = await GenerateDecisionPrompt(applicationId);

        var chatRequest = new ChatRequest
        {
            //Model = "gpt-4-1106-preview",
            Model = "gpt-3.5-turbo-1106",
            Temperature = 1,
            MaxTokens = 312,
            ResponseFormat = ChatRequest.ResponseFormats.JsonObject,
            TopP = 1,
            Messages = new[]
            {
                new ChatMessage(ChatMessageRole.System,
                    "No yapping in the response. You're an AI exert tasked with making a final decision on a candidate for an internship. Candidate can go thru multiple steps: screening, interview, assessment. For each step, Recruiter - Company leaves a review and a score, also the data gets sent to a chat GPT API and also receives a chat gpt score for the step. You will receive the following data from completed steps: AiScore, CompanyScore and Company review, also the description about the internship itself. You will also receive the most important data input: the final review of the candidate from the company. Your task is to evaluate the candidate's performance based on the provided review and task details and assign a final decision. The final decision should be in the range of 1 (indicating poor performance) to 5 (indicating excellent performance), also write your decision as text in the end of the summary. You must consider everything in the recruiter's review and how well the candidate fulfilled the requirements of the assigned tasks, the decision should mostly come from the final recruiter review, but also take into account the previous steps as stated before. Also write a review or summary of all the stages and how the candidate did. Provide output in valid JSON format. The JSON should be in this format: {\n  \"finalDecision\": {finalDecisionScore},\n  \"stagesReview\": {stages review text}\n}"),
                new ChatMessage(ChatMessageRole.User, prompt)
            }
        };

        var result = await api.Chat.CreateChatCompletionAsync(chatRequest);

        return JsonConvert.DeserializeObject<DecisionResponse>(result.ToString());
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
                    " and any additional notes/review provided by the recruitet. The data provided to you, will be in JSON format.  Your task is to evaluate the " +
                    "interview and assign an interview score. The interview score should be in the range of 1 (indicating poor " +
                    "performance) to 5 (indicating excellent performance). You must take into account everything in the recruiter notes/review, " +
                    "and how fit this person would be for the internship description. Provide output in valid JSON format. The JSON should be in " +
                    "this format: {\"interviewScore\": {score}}."),
                new ChatMessage(ChatMessageRole.User, prompt)
            }
        };

        var result = await api.Chat.CreateChatCompletionAsync(chatRequest);

        return JsonConvert.DeserializeObject<InterviewScoreResponse>(result.ToString());
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
                    " about the tasks assigned to the candidate and the recruiter's review of their performance. The data provided to you, will be in JSON format.  Your task is to evaluate the candidate's " +
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