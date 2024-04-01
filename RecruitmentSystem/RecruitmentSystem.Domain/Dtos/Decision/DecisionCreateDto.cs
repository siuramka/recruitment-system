namespace RecruitmentSystem.Domain.Dtos.Decision;

public class DecisionCreateDto
{
    public string CompanySummary { get; set; } = string.Empty;
    public int CompanyScore { get; set; }
}