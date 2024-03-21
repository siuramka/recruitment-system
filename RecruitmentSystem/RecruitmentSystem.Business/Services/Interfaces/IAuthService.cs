namespace RecruitmentSystem.Business.Services.Interfaces;

public interface IAuthService
{
    Task<bool> AuthorizeApplicationCreatorOrCompany(Guid applicationId, string userId);
    Task<bool> AuthorizeInternshipCompany(Guid internshipId, string userId);
}