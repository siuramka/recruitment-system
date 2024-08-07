using Microsoft.AspNetCore.Identity;
using Moq;
using Moq.EntityFrameworkCore;
using RecruitmentSystem.Business.Interfaces;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Tests.Services;

[TestFixture]
[TestOf(typeof(AuthService))]
public class AuthServiceTest
{
    private IAuthService _authService;
    private Mock<RecruitmentDbContext> _db;
    private Mock<UserManager<SiteUser>> _userManager;
    public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        mgr.Object.UserValidators.Add(new UserValidator<TUser>());
        mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

        return mgr;
    }
    
    [SetUp]
    public void Setup()
    {
        _db = new Mock<RecruitmentDbContext>();
        _userManager = MockUserManager(new List<SiteUser>());
        _authService = new AuthService(_db.Object, _userManager.Object);
    }
    
    
    [Test]
    public void AuthorizeApplicationCreatorOrCompany_ShouldAuthorize()
    {
        var user = new SiteUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "User1",
            CompanyId = Guid.NewGuid(),
        };

        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        
        var application = new Application()
        {
            Id = Guid.NewGuid(),
            SiteUser = user,
            InternshipId = Guid.NewGuid(),
        };
        
        var internship = new Internship()
        {
            Id = application.InternshipId,
            CompanyId = (Guid)user.CompanyId,
        };
        
        _db.Setup(x => x.Applications).ReturnsDbSet(new List<Application> { application });
        
        _db.Setup(x => x.Internships).ReturnsDbSet(new List<Internship> { internship });
        
        var result = _authService.AuthorizeApplicationCreatorOrCompany(application.Id, user.Id).Result;
        
        Assert.That(result, Is.True);
    }

    [Test]
    public void AuthorizeInternshipCompany_ShouldAuthorize()
    {
        var user = new SiteUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "User1",
            CompanyId = Guid.NewGuid(),
        };

        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        
        var internship = new Internship()
        {
            Id = Guid.NewGuid(),
            CompanyId = (Guid)user.CompanyId,
        };
        
        _db.Setup(x => x.Internships).ReturnsDbSet(new List<Internship> { internship });
        
        var result = _authService.AuthorizeInternshipCompany(internship.Id, user.Id).Result;
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void AuthorizeApplicationCompany_ShouldAuthorize()
    {
        var user = new SiteUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "User1",
            CompanyId = Guid.NewGuid(),
        };

        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        
        var application = new Application()
        {
            Id = Guid.NewGuid(),
            SiteUser = new SiteUser(),
            InternshipId = Guid.NewGuid(),
        };
        
        var internship = new Internship()
        {
            Id = application.InternshipId,
            CompanyId = (Guid)user.CompanyId,
        };
        
        _db.Setup(x => x.Applications).ReturnsDbSet(new List<Application> { application });
        
        _db.Setup(x => x.Internships).ReturnsDbSet(new List<Internship> { internship });
        
        var result = _authService.AuthorizeApplicationCompany(application.Id, user.Id).Result;
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void AuthorizeApplicationCompany_ShouldNotAuthorize()
    {
        var user = new SiteUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "User1",
            CompanyId = Guid.NewGuid(),
        };

        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new SiteUser());
        
        var application = new Application()
        {
            Id = Guid.NewGuid(),
            SiteUser = new SiteUser(),
            InternshipId = Guid.NewGuid(),
        };
        
        var internship = new Internship()
        {
            Id = application.InternshipId,
            CompanyId = (Guid)user.CompanyId,
        };
        
        _db.Setup(x => x.Applications).ReturnsDbSet(new List<Application> { application });
        
        _db.Setup(x => x.Internships).ReturnsDbSet(new List<Internship> { internship });
        
        var result = _authService.AuthorizeApplicationCompany(application.Id, user.Id).Result;
        
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task AuthorizeApplicationCreator_ShouldAuthorize()
    {
        var user = new SiteUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "User1",
            CompanyId = Guid.NewGuid(),
        };

        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        
        var application = new Application()
        {
            Id = Guid.NewGuid(),
            SiteUser = user,
            InternshipId = Guid.NewGuid(),
        };
        
        _db.Setup(x => x.Applications).ReturnsDbSet(new List<Application> { application });
        
        var result = await _authService.AuthorizeApplicationCreator(application.Id, user.Id);
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public async Task AuthorizeApplicationCreator_ShouldNotAuthorize()
    {
        var user = new SiteUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "User1",
            CompanyId = Guid.NewGuid(),
        };

        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new SiteUser());
        
        var application = new Application()
        {
            Id = Guid.NewGuid(),
            SiteUser = user,
            InternshipId = Guid.NewGuid(),
        };
        
        _db.Setup(x => x.Applications).ReturnsDbSet(new List<Application> { application });
        
        var result = await _authService.AuthorizeApplicationCreator(application.Id, user.Id);
        
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task AuthorizeEvaluationCreator_ShouldAuthorize()
    {
        var user = new SiteUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "User1",
            CompanyId = Guid.NewGuid(),
        };

        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        
        var application = new Application()
        {
            Id = Guid.NewGuid(),
            SiteUser = user,
            InternshipId = Guid.NewGuid(),
        };
        
        var evaluation = new Evaluation()
        {
            Id = Guid.NewGuid(),
            Application = application,
        };
        
        _db.Setup(x => x.Evaluations).ReturnsDbSet(new List<Evaluation> { evaluation });
        
        var result = await _authService.AuthorizeEvaluationCreator(evaluation.Id, user.Id);
        
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task AuthorizeEvaluationCompany_ShouldAuthorize()
    {
        var user = new SiteUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "User1",
            CompanyId = Guid.NewGuid(),
        };

        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

        var internship = new Internship()
        {
            Id = Guid.NewGuid(),
            CompanyId = (Guid)user.CompanyId,
        };

        var application = new Application()
        {
            Id = Guid.NewGuid(),
            SiteUser = user,
            InternshipId = internship.Id,
            Internship = internship
        };

        var evaluation = new Evaluation()
        {
            Id = Guid.NewGuid(),
            ApplicationId = application.Id,
            Application = application,
        };

        _db.Setup(x => x.Evaluations).ReturnsDbSet(new List<Evaluation> { evaluation });

        var result = await _authService.AuthorizeEvaluationCompany(evaluation.Id, user.Id);

        Assert.That(result, Is.True);
    }
}

