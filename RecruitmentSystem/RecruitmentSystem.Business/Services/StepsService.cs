using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Business.Interfaces;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Services;

public class StepsService : IStepsService
{
    private RecruitmentDbContext _db;

    public StepsService(RecruitmentDbContext db)
    {
        _db = db;
    }

    public async Task<List<InternshipStep>> GetInternshipSteps(Guid internshipId)
    {
        return  await _db.InternshipSteps
                .Include(internshipStep => internshipStep.Internship)
                .Include(internshipStep => internshipStep.Step)
                .Where(internshipStep => internshipStep.InternshipId == internshipId)
                .ToListAsync();
    }

    public async Task<InternshipStep?> GetInternshpStepByType(Guid internshipId, string stepName)
    {
       return await  _db.InternshipSteps
            .Include(internshipStep => internshipStep.Internship)
            .Include(internshipStep => internshipStep.Step)
            .Where(internshipStep => internshipStep.InternshipId == internshipId)
            .FirstOrDefaultAsync(internshipStep =>
                internshipStep.Step.Name.Equals(stepName));
    }
    
}