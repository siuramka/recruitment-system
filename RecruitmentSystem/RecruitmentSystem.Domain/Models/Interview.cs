namespace RecruitmentSystem.Domain.Models;

public class Interview
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public Application Application { get; set; }
    public DateTime StartTime { get; set; }
    public string Instructions { get; set; }
    public int MinutesLength { get; set; }
    
    public Guid? EvaluationId { get; set; }

}