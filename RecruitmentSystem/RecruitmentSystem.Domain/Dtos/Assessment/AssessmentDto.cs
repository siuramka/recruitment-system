namespace RecruitmentSystem.Domain.Dtos.Assessment;

public class AssessmentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}