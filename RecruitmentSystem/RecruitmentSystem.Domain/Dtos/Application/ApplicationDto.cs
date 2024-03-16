using RecruitmentSystem.Domain.Dtos.Internship;

namespace RecruitmentSystem.Domain.Dtos.Application;

public class ApplicationDto
{
    public Guid Id { get; set; }
    public int AiScore { get; set; }
    public int CompanyScore { get; set; }
    public int Score { get; set; }
    public InternshipDto InternshipDto { get; set; }
}