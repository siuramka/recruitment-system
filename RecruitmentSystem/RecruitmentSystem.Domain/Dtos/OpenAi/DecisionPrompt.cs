using RecruitmentSystem.Domain.Dtos.Decision;

namespace RecruitmentSystem.Domain.Dtos.OpenAi;

public class DecisionPrompt
{
    public string InternshipDescription { get; set; }
    public string StepEvaluations { get; set; }
    public string FinalCompanyReviewOfCandidate { get; set; }
}
