using RecruitmentSystem.Domain.Dtos.Assessment;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Interfaces;

public interface IAssessmentService
{
    Task<Assessment> CreateAssessment(Guid applicationId, AssessmentCreateDto assessmentCreateDto);
}