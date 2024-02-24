namespace RecruitmentSystem.Domain.Models;

public class InternshipStep
{
    public Guid StepId { get; set; }
    public Step Step { get; set; }
    
    public Guid InternshipId { get; set; }
    public Internship Internship { get; set; }
    
    public int PositionAscending { get; set; }
}