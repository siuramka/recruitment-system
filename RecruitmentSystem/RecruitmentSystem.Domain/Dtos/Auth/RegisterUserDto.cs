namespace RecruitmentSystem.Domain.Dtos.Auth;

public class RegisterUserDto
{
    public string Email { get; set; }
    public DateTime DateOfBirth { get; set; } = DateTime.Now;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Location { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}