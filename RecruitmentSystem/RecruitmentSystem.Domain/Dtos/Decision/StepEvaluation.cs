namespace RecruitmentSystem.Domain.Dtos.Decision;

public class StepEvaluation
{
    public string StepName { get; set; }
    public double AiScoreForCandidateInStep { get; set; }
    public double CompanyScoreForCandidateInStep { get; set; }
    public string CompanysReviewOfCandidateInStep { get; set; }
}