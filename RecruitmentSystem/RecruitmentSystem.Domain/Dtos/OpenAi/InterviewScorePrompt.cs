namespace RecruitmentSystem.Domain.Dtos.OpenAi;

public class InterviewScorePrompt
{
    public string InterviewReviewContent { get; set; }
    public int InterviewCompanyScore { get; set; }
    public string InternshipDescription { get; set; }
}