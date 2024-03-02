using RecruitmentSystem.Domain.Dtos.Application;

namespace RecruitmentSystem.Domain.Dtos.Internship;

public class InternshipCreateDto
{
    public string Name { get; set; }
    public string ContactEmail { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public string Requirements { get; set; }
    public bool IsPaid { get; set; }
    public bool IsRemote { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SlotsAvailable { get; set; }
    public string Skills { get; set; }
    
    public List<ApplicationStepDto> InternshipStepDtos { get; set; }
}