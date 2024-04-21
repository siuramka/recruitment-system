using RecruitmentSystem.Domain.Dtos.Decision;
using RecruitmentSystem.Domain.Dtos.Evaluation;
using RecruitmentSystem.Domain.Dtos.OpenAi;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Interfaces;

public interface IEvaluationService
{
    Task<List<Evaluation>> GetApplicationEvaluations(Guid applicationId);
    Task<List<StepEvaluation>> GetStepEvaluations(Guid applicationId);
    Task<Decision?> GetFinalDecision(Guid applicationId);
    Task<FinalScore> CalculateFinalScore(Guid applicationId);
    Task EvaluateInterviewAiScore(Guid interviewId);
    Task EvaluateAssessmentAiScore(Guid assessmentId);
    Task UpdateEvaluationAiScore(Evaluation evaluation, int score);
    Task CreateEvaluationWithAiScore(Application application, int score);
    Task<Evaluation> CreateEvaluation(EvaluationCreateDto evaluationCreateDto, Guid applicationId);
    Task AssignAssessmentEvaluation(Guid assessmentId, Guid evaluationId);
    Task AssignInterviewEvaluation(Guid interviewId, Guid evaluationId);
    Task UpdateScreeningCompanyEvaluation(Guid? evaluationId, EvaluationCreateDto evaluationCreateDto);
    Task UpdateDecisionWithAiReview(DecisionResponse decisionResponse, Decision decision);
    Task UpdateDecisionWithFitnessReview(FintessReviewResponse fintessReviewResponse, Decision decision);
    Task<string> GetEvaluationStepName(Evaluation evaluation);
    double CalculateCorrelation(int[] aiScores, int[] companyScores);
    int[] GetCompanyScores(List<StepEvaluation> stepEvaluations, Decision finalDecision);
    int[] GetAiScores(List<StepEvaluation> stepEvaluations, Decision finalDecision);
    Task<Setting> GetApplicationInternshipSetting(Guid applicaitonId);
}