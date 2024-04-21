using Moq;
using Moq.EntityFrameworkCore;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Tests.Services;

public class DecisionServiceTests
{
    [Test]
    public async Task CreateDecision_ShouldCreate()
    {
        var decision = new Decision();
        var db = new Mock<RecruitmentDbContext>();
        var decisionService = new DecisionService(db.Object);
        db.Setup(d => d.Decisions).ReturnsDbSet([]);
        
        await decisionService.CreateDecision(decision);
        
        db.Verify(x => x.Decisions.AddAsync(decision,default), Times.Once);
        db.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
    
    [Test]
    public async Task Remove_ShouldRemove()
    {
        var decision = new Decision();
        var db = new Mock<RecruitmentDbContext>();
        var decisionService = new DecisionService(db.Object);
        db.Setup(d => d.Decisions).ReturnsDbSet([decision]);
        
        await decisionService.Remove(decision);
        
        db.Verify(x => x.Decisions.Remove(decision), Times.Once);
        db.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}