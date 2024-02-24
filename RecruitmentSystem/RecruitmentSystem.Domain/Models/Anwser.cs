namespace RecruitmentSystem.Domain.Models;

public class Anwser
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public bool IsSuccessful { get; set; }
    
    public Guid ApplicationId { get; set; }
    public Application Application { get; set; }
}