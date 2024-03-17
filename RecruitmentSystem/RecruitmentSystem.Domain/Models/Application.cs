namespace RecruitmentSystem.Domain.Models;

public class Application
{
    public Guid Id { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public SiteUser SiteUser { get; set; }
    
    public Guid InternshipId { get; set; }
    
    public Internship Internship { get; set; }
    
    
    public Guid InternshipStepId { get; set; }
    
    public InternshipStep InternshipStep { get; set; }
    
    public string Skills { get; set; } = string.Empty;
    
    public Assessment? Assessment { get; set; }
    
    public Review? Review { get; set; }
    
    public int Score { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public ScoreStatus ScoreStatus { get; set; }
    
    public List<Evaluation> Evaluations { get; set; }
}
