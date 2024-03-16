namespace RecruitmentSystem.Domain.Models;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; } 
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Website { get; set; }
    
    public int BadReviews { get; set; }
    
    public int NeutralReviews { get; set; }
    
    public int PositiveReviews { get; set; }
    
    public DateTime AverageTime { get; set; }
    
    public ICollection<Internship> Internships { get; set; }
    public SiteUser SiteUser { get; set; }
    public ICollection<Review> Reviews { get; set; }
}