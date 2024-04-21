using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Interfaces;

public interface IApplicationService
{
    Task EndApplication(Application application);
    Task<List<Application>> GetUserApplications( SiteUser siteUser);
    Task<List<Application>> GetInternshipApplications(Guid internshipId);
    Task<List<Application>> GetDecisionApplications(Guid? companyId);
    Task<Application?> GetApplicaitonById(Guid applicationId);
    Task<Application?> IsApplicationCreated(Guid internshipId, string siteUserId);
    Task CreateApplication(Application application);
    Task UpdateApplication(Application application);
}