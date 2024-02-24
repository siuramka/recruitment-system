namespace RecruitmentSystem.Domain.Dtos.Company;

public class CompanyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Website { get; set; }
}