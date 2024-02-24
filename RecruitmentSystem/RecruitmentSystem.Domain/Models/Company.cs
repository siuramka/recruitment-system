namespace RecruitmentSystem.Domain.Models;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; } //City country
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Website { get; set; }
    
    public ICollection<Internship> Internships { get; set; }
}