using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Tests.Services;

[TestFixture]
[TestOf(typeof(ApplicationService))]
public class ApplicationServiceTest
{
    private IApplicationService _applicationService;
    private Mock<RecruitmentDbContext> _db;


    [SetUp]
    public void Setup()
    {
        _db = new Mock<RecruitmentDbContext>();
        _applicationService = new ApplicationService(_db.Object);
    }

    [Test]
    public async Task EndApplication_ShouldEnd()
    {
        // Arrange
        var application = new Application();

        // Act
        await _applicationService.EndApplication(application);

        // Assert
        Assert.That(application.EndTime, Is.LessThan(DateTime.Now.AddSeconds(-30)));
    }

    [Test]
    public async Task GetUserApplications_ReturnsCorrectApplications()
    {
        // Arrange
        var siteUser = new SiteUser();
        var applications = new List<Application>
        {
            new Application { SiteUser = siteUser },
            new Application { SiteUser = new SiteUser() },
            new Application { SiteUser = siteUser }
        };

        _db.Setup<DbSet<Application>>(x => x.Applications)
            .ReturnsDbSet(applications);

        // Act
        var result = await _applicationService.GetUserApplications(siteUser);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task GetInternshipApplications_ReturnsCorrectApplications()
    {
        // Arrange
        var internshipId = Guid.NewGuid();
        var applications = new List<Application>
        {
            new Application { InternshipId = internshipId },
            new Application { InternshipId = Guid.NewGuid() },
            new Application { InternshipId = internshipId }
        };

        _db.Setup<DbSet<Application>>(x => x.Applications)
            .ReturnsDbSet(applications);

        // Act
        var result = await _applicationService.GetInternshipApplications(internshipId);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task GetApplicationById_ReturnsApplication()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        var application = new Application { Id = applicationId };
        var applications = new List<Application>
        {
            application,
            new Application { Id = Guid.NewGuid() },
            new Application { Id = Guid.NewGuid() }
        };

        _db.Setup<DbSet<Application>>(x => x.Applications)
            .ReturnsDbSet(applications);

        // Act
        var result = await _applicationService.GetApplicaitonById(applicationId);

        // Assert
        Assert.That(result, Is.EqualTo(application));
    }

    [Test]
    public async Task GetDecisionApplications_ReturnsCompanyApplications()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var applications = new List<Application>
        {
            new Application
            {
                Internship = new Internship { CompanyId = companyId },
                InternshipStep = new InternshipStep { Step = new Step { StepType = StepType.Interview } }
            },
            new Application
            {
                Internship = new Internship { CompanyId = Guid.NewGuid() },
                InternshipStep = new InternshipStep { Step = new Step { StepType = StepType.Assessment } }
            },
            new Application
            {
                Internship = new Internship { CompanyId = companyId },
                InternshipStep = new InternshipStep { Step = new Step { StepType = StepType.Decision } }
            }
        };

        _db.Setup<DbSet<Application>>(x => x.Applications)
            .ReturnsDbSet(applications);

        // Act
        var result = await _applicationService.GetDecisionApplications(companyId);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task IsApplicationCreated_ReturnsApplication()
    {
        // Arrange
        var internshipId = Guid.NewGuid();
        var siteUserId = Guid.NewGuid().ToString();
        var application = new Application
            { Internship = new Internship { Id = internshipId }, SiteUser = new SiteUser { Id = siteUserId } };
        var applications = new List<Application>
        {
            application,
            new Application
            {
                Internship = new Internship { Id = Guid.NewGuid() },
                SiteUser = new SiteUser { Id = Guid.NewGuid().ToString() }
            },
            new Application
            {
                Internship = new Internship { Id = internshipId },
                SiteUser = new SiteUser { Id = Guid.NewGuid().ToString() }
            }
        };

        _db.Setup<DbSet<Application>>(x => x.Applications)
            .ReturnsDbSet(applications);

        // Act
        var result = await _applicationService.IsApplicationCreated(internshipId, siteUserId);

        // Assert
        Assert.That(result, Is.EqualTo(application));
    }

    [Test]
    public async Task CreateApplication_CreatesApplication()
    {
        // Arrange
        var application = new Application() { Id = Guid.NewGuid() };

        var applications = new List<Application>
        {
            application,
        };

        _db.Setup<DbSet<Application>>(x => x.Applications)
            .ReturnsDbSet(applications);

        // Act
        await _applicationService.CreateApplication(application);

        var created = _db.Object.Applications.FirstOrDefault();

        // Assert
        Assert.That(created.Id, Is.EqualTo(application.Id));
    }

    [Test]
    public async Task UpdateApplication_UpdatesApplication()
    {
        // Arrange
        var application = new Application() { Id = Guid.NewGuid() };

        var applications = new List<Application>
        {
            application,
        };

        _db.Setup<DbSet<Application>>(x => x.Applications)
            .ReturnsDbSet(applications);

        // Act
        application.Id = Guid.NewGuid();
        await _applicationService.UpdateApplication(application);
        
        var updated = _db.Object.Applications.FirstOrDefault();

        // Assert
        Assert.That(updated.Id, Is.EqualTo(application.Id));
    }
}