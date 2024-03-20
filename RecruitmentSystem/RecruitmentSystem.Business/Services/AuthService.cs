using RecruitmentSystem.DataAccess;

namespace RecruitmentSystem.Business.Services;

public class AuthService
{
    private RecruitmentDbContext _db;

    public AuthService(RecruitmentDbContext db)
    {
        _db = db;
    }
    
}