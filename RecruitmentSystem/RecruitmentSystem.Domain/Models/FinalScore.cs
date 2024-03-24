namespace RecruitmentSystem.Domain.Models;

public class FinalScore
{
    public Guid Id { get; set; }
    
    public Guid ApplicationId { get; set; }
    public Application Application { get; set; }
    
    public double Score { get; set; }
    
    public double CompanyScoreX2 { get; set; }
    
    public double AiScoreX1 { get; set; }
    
    public double X1X2Average { get; set; }
    
    public double Correlation { get; set; }
    public double CorrelationBoostValue { get; set; }
    public double CorrelationBoostModifer { get; set; }
    
    public string Review { get; set; } = string.Empty;
}