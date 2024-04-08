using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Services;

public interface IAuthService
{
    Task<bool> AuthorizeApplicationCreatorOrCompany(Guid applicationId, string userId);
    Task<bool> AuthorizeInternshipCompany(Guid internshipId, string userId);
    Task<bool> AuthorizeApplicationCompany(Guid applicationId, string userId);
    Task<bool> AuthorizeApplicationCreator(Guid applicationId, string userId);
    Task<bool> AuthorizeEvaluationCompany(Guid evaluationId, string userId);
    Task<bool> AuthorizeEvaluationCreator(Guid evaluationId, string userId);
}

public class AuthService : IAuthService
{
    private RecruitmentDbContext _db;
    private UserManager<SiteUser> _userManager;

    public AuthService(RecruitmentDbContext db, UserManager<SiteUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }
    
    public async Task<bool> AuthorizeApplicationCreatorOrCompany(Guid applicationId, string userId)
    {
        var siteUser = await _userManager.FindByIdAsync(userId);
        
        var application = await _db.Applications
            .Include(ap => ap.SiteUser)
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));
        
        var internship = await _db.Internships
            .FirstOrDefaultAsync(i => i.Id.Equals(application.InternshipId));
        
        return internship.CompanyId == siteUser.CompanyId || application.SiteUser.Id == userId;
    }
    
    public async Task<bool> AuthorizeApplicationCreator(Guid applicationId, string userId)
    {
        var siteUser = await _userManager.FindByIdAsync(userId);
        
        var application = await _db.Applications
            .Include(ap => ap.SiteUser)
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));
        
        return application.SiteUser.Id == siteUser.Id;
    }

    public async Task<bool> AuthorizeEvaluationCreator(Guid evaluationId, string userId)
    {
        var siteUser = await _userManager.FindByIdAsync(userId);

        var evaluation = await _db.Evaluations
            .Include(a => a.Application)
            .ThenInclude(a => a.SiteUser)
            .FirstOrDefaultAsync(e => e.Id == evaluationId);
        
        return evaluation.Application.SiteUser.Id == siteUser.Id;
    }
    
    public async Task<bool> AuthorizeEvaluationCompany(Guid evaluationId, string userId)
    {
        var siteUser = await _userManager.FindByIdAsync(userId);

        var evaluation = await _db.Evaluations
            .Include(a => a.Application)
            .ThenInclude(a => a.Internship)
            .FirstOrDefaultAsync(e => e.Id == evaluationId);
        
        return evaluation.Application.Internship.CompanyId == siteUser.CompanyId;
    }

    public async Task<bool> AuthorizeInternshipCompany(Guid internshipId, string userId)
    {
        var siteUser = await _userManager.FindByIdAsync(userId);
        
        var internship = await _db.Internships
            .FirstOrDefaultAsync(i => i.Id.Equals(internshipId));
        
        return internship.CompanyId == siteUser.CompanyId;
    }

    public async Task<bool> AuthorizeApplicationCompany(Guid applicationId, string userId)
    {
        var siteUser = await _userManager.FindByIdAsync(userId);
        
        var application = await _db.Applications
            .Include(ap => ap.SiteUser)
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));
        
        var internship = await _db.Internships
            .FirstOrDefaultAsync(i => i.Id.Equals(application.InternshipId));

        return internship.CompanyId == siteUser.CompanyId;
    }
}