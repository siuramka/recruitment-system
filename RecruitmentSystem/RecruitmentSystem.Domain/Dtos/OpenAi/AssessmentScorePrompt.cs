namespace RecruitmentSystem.Domain.Dtos.OpenAi;

public class AssessmentScorePrompt
{
    public string AssessmentReviewContent { get; set; }

    public int AssessmentCompanyScore { get; set; }

    public string InternshipDescription { get; set; }
}