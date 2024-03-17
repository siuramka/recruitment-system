namespace RecruitmentSystem.Domain.Dtos.Decision;

public class StepEvaluation
{
    public string StepName { get; set; }
    public int AiScoreForCandidateInStep { get; set; }
    public int CompanyScoreForCandidateInStep { get; set; }
    public string CompanysReviewOfCandidateInStep { get; set; }
}