namespace RecruitmentSystem.Domain.Models;

public class InternshipStep
{
    public Guid Id { get; set; }
    public Guid StepId { get; set; }
    public Step Step { get; set; }
    
    public Guid InternshipId { get; set; }
    public Internship Internship { get; set; }
    
    public int PositionAscending { get; set; }
    
    public List<Application> Applications { get; set; }
}