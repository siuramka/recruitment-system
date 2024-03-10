using RecruitmentSystem.Domain.Dtos.Internship;

namespace RecruitmentSystem.Domain.Dtos.Application;

public class ApplicationDto
{
    public Guid Id { get; set; }
    public InternshipDto InternshipDto { get; set; }
}