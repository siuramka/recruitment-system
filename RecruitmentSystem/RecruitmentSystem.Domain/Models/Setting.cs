namespace RecruitmentSystem.Domain.Models;

public class Setting
{
    public Guid Id { get; set; }
    
    public int AiScoreWeight { get; set; }
    public int CompanyScoreWeight { get; set; }
    public int TotalScoreWeight { get; set; }
    
    public Guid InternshipId { get; set; }
    public Internship Internship { get; set; }
}