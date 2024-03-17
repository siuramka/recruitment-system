using System.ComponentModel.DataAnnotations;

namespace RecruitmentSystem.Domain.Models;

public class Internship
{
    public Guid Id { get; set; }
    
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public bool IsPaid { get; set; }
    public bool IsRemote { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SlotsAvailable { get; set; }
    public int TakenSlots { get; set; }
    public string Skills { get; set; } = string.Empty;
    
    public List<InternshipStep> InternshipSteps { get; set; }
}