namespace RecruitmentSystem.Domain.Models;

public class Decision
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public Application Application { get; set; }
    public int AiStagesScore { get; set; }
    public int CompanyStagesScores { get; set; }
    public string AiStagesReview { get; set; } = string.Empty;
    
    public string AiCandidateSummary { get; set; } = string.Empty;
    public string CompanySummary { get; set; } = string.Empty;
}