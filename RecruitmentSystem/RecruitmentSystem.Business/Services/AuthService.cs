using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Business.Services.Interfaces;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Services;

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
    
    public async Task<bool> AuthorizeInternshipCompany(Guid internshipId, string userId)
    {
        var siteUser = await _userManager.FindByIdAsync(userId);
        
        var internship = await _db.Internships
            .FirstOrDefaultAsync(i => i.Id.Equals(internshipId));
        
        return internship.CompanyId == siteUser.CompanyId;
    }
    
}