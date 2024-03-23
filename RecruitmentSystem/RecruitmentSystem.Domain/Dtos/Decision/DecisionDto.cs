namespace RecruitmentSystem.Domain.Dtos.Decision;

public class DecisionDto
{
    public Guid Id { get; set; }
    
    public int AiStagesScore { get; set; }
    
    public int CompanyScore { get; set; }

    public string AiStagesReview { get; set; } = string.Empty;
    
    public string AiCandidateSummary { get; set; } = string.Empty;

    public string CompanySummary { get; set; } = string.Empty;
}