namespace RecruitmentSystem.Domain.Dtos.SiteUser;

public class SiteUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Location { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}