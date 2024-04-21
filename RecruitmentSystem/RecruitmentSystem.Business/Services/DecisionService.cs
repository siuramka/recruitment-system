using RecruitmentSystem.Business.Interfaces;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Services;

public class DecisionService : IDecisionService
{
    private RecruitmentDbContext _db;
    public DecisionService(RecruitmentDbContext db)
    {
        _db = db;
    }

    public async Task CreateDecision(Decision decision)
    {
        await _db.Decisions.AddAsync(decision);
        await _db.SaveChangesAsync();
    }
    
    public async Task Remove(Decision decision)
    {
        _db.Decisions.Remove(decision);
        await _db.SaveChangesAsync();
    }
}