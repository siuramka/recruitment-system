using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Domain.Dtos.Application;

public class ApplicationCreateDto
{
    public InternshipStep InternshipStep { get; set; }
    
    public string Skills { get; set; }
}