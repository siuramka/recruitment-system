using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using RecruitmentSystem.API.Mappings;
using RecruitmentSystem.Business.Interfaces;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Tests.Services;

[TestFixture]
[TestOf(typeof(StepsService))]
public class StepsServiceTest
{
    private IStepsService _stepsService;
    private Mock<RecruitmentDbContext> _db;

    [SetUp]
    public void Setup()
    {
        _db = new Mock<RecruitmentDbContext>();
        _stepsService = new StepsService(_db.Object);
    }

    [Test]
    public async Task GetInternshipSteps_ReturnsCorrectSteps_WhenInternshipIdIsValid()
    {
        var internshipId = Guid.NewGuid();
        var internshipSteps = new List<InternshipStep>
        {
            new InternshipStep
            {
                InternshipId = internshipId,
                Step = new Step
                {
                    Name = "Screening"
                }
            },
            new InternshipStep
            {
                InternshipId = internshipId,
                Step = new Step
                {
                    Name = "Interview"
                }
            }
        };

        _db.Setup(x => x.InternshipSteps)
            .ReturnsDbSet(internshipSteps);

        var result = await _stepsService.GetInternshipSteps(internshipId);

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].Step.Name, Is.EqualTo("Screening"));
            Assert.That(result[1].Step.Name, Is.EqualTo("Interview"));
        });
    }
    
    [Test]
    public async Task GetInternshpStepByType_ReturnsCorrectStep_WhenInternshipIdAndStepNameAreValid()
    {
        var internshipId = Guid.NewGuid();
        var internshipSteps = new List<InternshipStep>
        {
            new InternshipStep
            {
                InternshipId = internshipId,
                Step = new Step
                {
                    Name = "Screening"
                }
            },
            new InternshipStep
            {
                InternshipId = internshipId,
                Step = new Step
                {
                    Name = "Interview"
                }
            }
        };

        _db.Setup(x => x.InternshipSteps)
            .ReturnsDbSet(internshipSteps);

        var result = await _stepsService.GetInternshpStepByType(internshipId, "Screening");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Step.Name, Is.EqualTo("Screening"));
    }
}