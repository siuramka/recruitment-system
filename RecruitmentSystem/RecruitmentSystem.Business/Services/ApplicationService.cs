using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Services;

public class ApplicationService
{
    public RecruitmentDbContext _db;

    public ApplicationService(RecruitmentDbContext db)
    {
        _db = db;
    }

    public async Task EndApplication(Application application)
    {
        application.EndTime = DateTime.Now.ToUniversalTime();
        await _db.SaveChangesAsync();
    }
}