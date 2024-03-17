namespace RecruitmentSystem.Domain.Dtos.Decision;

public class DecisionDto
{
    public int AiStagesScore { get; set; }

    public string AiStagesReview { get; set; } = string.Empty;
    
    public string AiCandidateSummary { get; set; } = string.Empty;

    public string CompanySummary { get; set; } = string.Empty;
}