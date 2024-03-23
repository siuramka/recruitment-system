namespace RecruitmentSystem.Domain.Dtos.FinalScore;

public class FinalScoreDto
{
    public Guid Id { get; set; }
    
    public double Score { get; set; }
    
    public double CompanyScoreX2 { get; set; }
    
    public double AiScoreX1 { get; set; }
    
    public double Correlation { get; set; }
    
    public string Review { get; set; } = string.Empty;
}