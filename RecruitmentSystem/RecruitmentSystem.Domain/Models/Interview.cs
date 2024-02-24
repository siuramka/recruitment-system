namespace RecruitmentSystem.Domain.Models;

public class Interview
{
    public Guid Id { get; set; }
    public Guid InternshipId { get; set; }
    public Guid SiteUserId { get; set; }
}