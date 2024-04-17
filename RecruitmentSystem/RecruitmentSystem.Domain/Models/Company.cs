namespace RecruitmentSystem.Domain.Models;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; }  = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    
    public ICollection<Internship> Internships { get; set; }
    public SiteUser SiteUser { get; set; }
}