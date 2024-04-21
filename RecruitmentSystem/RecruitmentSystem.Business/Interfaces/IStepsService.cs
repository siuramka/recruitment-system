using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Interfaces;

public interface IStepsService
{
    Task<List<InternshipStep>> GetInternshipSteps(Guid internshipId);
    Task<InternshipStep?> GetInternshpStepByType(Guid internshipId, string stepName);
}