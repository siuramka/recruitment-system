namespace RecruitmentSystem.Domain.Models;

public class Application
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    
    public SiteUser SiteUser { get; set; }
    
    public Guid InternshipId { get; set; }
    public Internship Internship { get; set; }
    
    public InternshipStep InternshipStep { get; set; }
    
    public string Skills { get; set; }
}
