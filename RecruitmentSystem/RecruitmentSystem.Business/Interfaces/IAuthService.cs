namespace RecruitmentSystem.Business.Interfaces;

public interface IAuthService
{
    Task<bool> AuthorizeApplicationCreatorOrCompany(Guid applicationId, string userId);
    Task<bool> AuthorizeInternshipCompany(Guid internshipId, string userId);
    Task<bool> AuthorizeApplicationCompany(Guid applicationId, string userId);
    Task<bool> AuthorizeApplicationCreator(Guid applicationId, string userId);
    Task<bool> AuthorizeEvaluationCompany(Guid evaluationId, string userId);
    Task<bool> AuthorizeEvaluationCreator(Guid evaluationId, string userId);
}