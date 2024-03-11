namespace RecruitmentSystem.Domain.Dtos.Interview;

public class InterviewCreateDto
{
    public DateTime StartTime { get; set; }
    public string Instructions { get; set; }
    public int MinutesLength { get; set; }
}