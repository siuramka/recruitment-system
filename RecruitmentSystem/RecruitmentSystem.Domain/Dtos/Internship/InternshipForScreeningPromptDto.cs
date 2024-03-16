namespace RecruitmentSystem.Domain.Dtos.Internship;

public class InternshipForScreeningPromptDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Requirements { get; set; }
    public bool IsPaid { get; set; }
    public bool IsRemote { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Skills { get; set; }
}