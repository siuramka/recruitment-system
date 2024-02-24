namespace RecruitmentSystem.Domain.Models;

public class Step
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public StepType StepType { get; set; }
    
    public ICollection<InternshipStep> InternshipSteps { get; set; }
}