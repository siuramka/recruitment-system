using RecruitmentSystem.Domain.Dtos.Internship;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Services.Interfaces;

public interface IInternshipService
{
    Task<Internship> CreateInternshipAsync(InternshipCreateDto internshipCreateDto, string userId);
    Task<List<InternshipDto>> GetAllInternshipsAsDtoAsync();
    Task<List<InternshipDto>> GetAllInternshipsAsDtoOfCompanyAsync(string userId);
    Task<Internship?> GetInternshipByIdIncludeCompany(Guid internshipId);
}