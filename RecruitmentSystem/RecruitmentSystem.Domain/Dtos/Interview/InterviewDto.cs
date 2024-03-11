namespace RecruitmentSystem.Domain.Dtos.Interview;

public class InterviewDto
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public DateTime StartTime { get; set; }
    public string Instructions { get; set; }
    public int MinutesLength { get; set; }
}