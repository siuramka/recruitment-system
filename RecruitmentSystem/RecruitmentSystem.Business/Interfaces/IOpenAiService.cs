using RecruitmentSystem.Domain.Dtos.OpenAi;

namespace RecruitmentSystem.Business.Interfaces;

public interface IOpenAiService
{
    Task<string> GenerateScreeningPrompt(Guid applicaitonId);
    Task<string> GenerateDecisionPrompt(Guid applicationId);
    Task<ScreeningScoreResponse?> GetScreeningScore(Guid applicationId);
    Task<FintessReviewResponse?> GetFitReview(string prompt);
    Task<DecisionResponse?> GetFinalDecision(string prompt);
    Task<InterviewScoreResponse?> GetInterviewScore(Guid interviewId);
    Task<AssessmentScoreResponse?> GetAssessmentScore(Guid assessmentId);
}