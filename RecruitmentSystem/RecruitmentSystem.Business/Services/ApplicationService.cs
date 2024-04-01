using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Services;

public class ApplicationService
{
    private RecruitmentDbContext _db;

    public ApplicationService(RecruitmentDbContext db)
    {
        _db = db;
    }

    public async Task EndApplication(Application application)
    {
        application.EndTime = DateTime.Now.ToUniversalTime();
        await _db.SaveChangesAsync();
    }

    public async Task<List<Application>> GetUserApplications( SiteUser siteUser)
    {
       return await _db.Applications
            .Include(app => app.Internship)
            .ThenInclude(i => i.Company)
            .Include(app => app.SiteUser)
            .Include(app => app.InternshipStep)
            .ThenInclude(istep => istep.Step)
            .Where(app => app.SiteUser == siteUser)
            .ToListAsync();
    }

    public async Task<List<Application>> GetInternshipApplications(Guid internshipId)
    {
        return await _db.Applications
            .Where(app => app.InternshipId == internshipId)
            .Include(app => app.Internship)
            .ThenInclude(i => i.Company)
            .Include(app => app.SiteUser)
            .Include(app => app.InternshipStep)
            .ThenInclude(istep => istep.Step)
            .ToListAsync();
    }

    public async Task<List<Application>> GetDecisionApplications(Guid? companyId)
    {
       return await _db.Applications
            .Include(a => a.Internship)
            .ThenInclude(i => i.Company)
            .Where(a => a.Internship.CompanyId == companyId)
            .Include(app => app.SiteUser)
            .Include(app => app.InternshipStep)
            .ThenInclude(istep => istep.Step)
            .Where(app => app.InternshipStep.Step.StepType == StepType.Decision)
            .ToListAsync();
    }

    public async Task<Application?> GetApplicaitonById(Guid applicationId)
    {
        return await _db.Applications
            .Include(app => app.Internship)
            .ThenInclude(i => i.Company)
            .Include(app => app.SiteUser)
            .Include(app => app.InternshipStep)
            .ThenInclude(istep => istep.Step)
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));
    }

    public async Task<Application?> IsApplicationCreated(Guid internshipId, string siteUserId)
    {
        return await _db.Applications
            .Include(ap => ap.Internship)
            .Include(ap => ap.SiteUser)
            .FirstOrDefaultAsync(ap => ap.SiteUser.Id == siteUserId && ap.Internship.Id == internshipId );
    }

    public async Task CreateApplication(Application application)
    {
        _db.Applications.Add(application);
        await _db.SaveChangesAsync();
    }
}