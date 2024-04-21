using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using RecruitmentSystem.Business.Interfaces;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Assessment;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Tests.Services;

[TestFixture]
[TestOf(typeof(AssessmentService))]
public class AssessmentServiceTest
{
    private IAssessmentService _assessmentService;
    private Mock<RecruitmentDbContext> _db;


    [SetUp]
    public void Setup()
    {
        _db = new Mock<RecruitmentDbContext>();
        _assessmentService = new AssessmentService(_db.Object);
    }

    [Test]
    public void CreateAssessment_CreatesNewAssessment()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        var assessmentCreateDto = new AssessmentCreateDto
        {
            Content = "Content",
            EndTime = DateTime.Now
        };
        
        _db.Setup<DbSet<Assessment>>(x => x.Assessments)
            .ReturnsDbSet(new List<Assessment>());
        
        // Act
        var result = _assessmentService.CreateAssessment(applicationId, assessmentCreateDto).Result;
        
        // Assert
        Assert.That(result.ApplicationId, Is.EqualTo(applicationId));
        
    }
}