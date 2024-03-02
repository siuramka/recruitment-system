using System.ComponentModel.DataAnnotations;

namespace RecruitmentSystem.Domain.Models;

public class Internship
{
    public Guid Id { get; set; }
    
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    
    public string Name { get; set; }
    public string ContactEmail { get; set; }
    public string Address { get; set; }
    [MaxLength(5000)]
    public string Description { get; set; }
    [MaxLength(5000)]
    public string Requirements { get; set; }
    public bool IsPaid { get; set; }
    public bool IsRemote { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int SlotsAvailable { get; set; }
    public int TakenSlots { get; set; }
    [MaxLength(1000)]
    public string Skills { get; set; }
    
    public List<InternshipStep> InternshipSteps { get; set; }
}