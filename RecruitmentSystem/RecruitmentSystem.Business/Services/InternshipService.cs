using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Internship;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Services;

public class InternshipService : IInternshipService
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;

    public InternshipService(RecruitmentDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<Internship> CreateInternshipAsync(InternshipCreateDto internshipCreateDto, string userId)
    {
        var internship = _mapper.Map<Internship>(internshipCreateDto);

        var company = await _db.Companys
            .Include(c => c.SiteUser)
            .FirstOrDefaultAsync(c => c.SiteUser.Id == userId);

        internship.Company = company;
        internship.CreatedAt = DateTime.Now.ToUniversalTime();
        internship.StartDate = DateTime.Now.ToUniversalTime();
        internship.EndDate = DateTime.Now.ToUniversalTime(); 

        _db.Internships.Add(internship);
        await _db.SaveChangesAsync();

        List<InternshipStep> internshipSteps = new();

        foreach (var internshipStepDto in internshipCreateDto.InternshipStepDtos)
        {
            var stepType = Enum.Parse<StepType>(internshipStepDto.StepType);

            var step = await _db.Steps.FirstOrDefaultAsync(s => s.StepType == stepType);

            var internshipStep = new InternshipStep
            {
                InternshipId = internship.Id, StepId = step.Id, PositionAscending = internshipStepDto.PositionAscending
            };

            internshipSteps.Add(internshipStep);
        }

        internship.InternshipSteps = internshipSteps;
        await _db.SaveChangesAsync();

        var setting = _mapper.Map<Setting>(internshipCreateDto.SettingCreateDto);
        setting.InternshipId = internship.Id;
        _db.Add(setting);
        await _db.SaveChangesAsync();

        return internship;
    }
    
    public async Task<List<InternshipDto>> GetAllInternshipsAsDtoAsync()
    {
        return await _db.Internships
            .Include(i => i.Company)
            .Include(i => i.Setting)
            .Select(i => _mapper.Map<InternshipDto>(i))
            .ToListAsync();
    }

    public async Task<List<InternshipDto>> GetAllInternshipsAsDtoOfCompanyAsync(string userId)
    {
        var company = await _db.Companys
            .Include(c => c.SiteUser)
            .FirstOrDefaultAsync(c => c.SiteUser.Id == userId);

        if (company is null)
        {
           throw new Exception("Company not found");
        }

        var internships = await _db.Internships
            .Include(i => i.Company)
            .Include(i => i.Setting)
            .Where(internship => internship.CompanyId == company.Id)
            .Select(i => _mapper.Map<InternshipDto>(i))
            .ToListAsync();
        
        return internships;
    }
    
    public async Task<Internship?> GetInternshipByIdIncludeCompany(Guid internshipId)
    {
        return await _db.Internships
            .Include(i => i.Company)
            .FirstOrDefaultAsync(internship => internship.Id == internshipId);
    }
}