using RecruitmentSystem.Domain.Dtos.Internship;
using RecruitmentSystem.Domain.Dtos.SiteUser;

namespace RecruitmentSystem.Domain.Dtos.Application;

public class ApplicationListItemDto
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public int AiScore { get; set; }
    public int Score { get; set; }
    public int CompanyScore { get; set; }
    public SiteUserDto SiteUserDto { get; set; }
    public string StepName { get; set; }
    public InternshipDto InternshipDto { get; set; }
}