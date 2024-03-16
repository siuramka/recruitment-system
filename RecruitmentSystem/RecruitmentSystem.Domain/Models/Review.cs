namespace RecruitmentSystem.Domain.Models;

public class Review
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    
    public ReviewStatus ReviewStatus { get; set; }
    
    public Guid CompanyId { get; set; }
    
    public string Content { get; set; }
}