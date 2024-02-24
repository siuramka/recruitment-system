using Microsoft.AspNetCore.Identity;

namespace RecruitmentSystem.Domain.Models;

public class SiteUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Location { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }
}