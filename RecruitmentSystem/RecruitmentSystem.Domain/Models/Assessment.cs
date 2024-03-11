namespace RecruitmentSystem.Domain.Models;

public class Assessment
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public Application Application { get; set; }
    
    public string Content { get; set; }
}