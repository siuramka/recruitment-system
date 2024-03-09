using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.DataAccess.Seeders;

public class DataSeeder
{
    private RecruitmentDbContext _dbContext;

    public DataSeeder(RecruitmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedInternship()
    {
        if (_dbContext.Internships.Any())
        {
            return;
        }

        _dbContext.SaveChanges();

        var company = _dbContext.Companys.First();
        _dbContext.Internships.Add(new Internship()
        {
            Company = company,
            Name = "Software JAVA Internship",
            CreatedAt = DateTime.Today.ToUniversalTime(),
            Address = "Kaunas",
            StartDate = DateTime.Today.ToUniversalTime(),
            EndDate = DateTime.Today.ToUniversalTime(),
            Description = "COOL COOL COOL COOL COOL COOL COOL COOL COOL COOL COOL ",
            ContactEmail = "hr@company.com",
            Requirements = "ZAZA BAGAG MAGA",
            Skills = "MOUTH GOOD MOORNING",
            IsPaid = true,
            IsRemote = true,
            SlotsAvailable = 5,
            TakenSlots = 0,
        });
        _dbContext.SaveChanges();

        var internship = _dbContext.Internships.First();

        _dbContext.InternshipSteps.AddRange(
            new InternshipStep()
            {
                Step = _dbContext.Steps.First(x => x.StepType == StepType.Screening),
                Internship = internship,
                PositionAscending = 0
            },
            new InternshipStep()
            {
                Step = _dbContext.Steps.First(x => x.StepType == StepType.Interview),
                Internship = internship,
                PositionAscending = 1
            },
            new InternshipStep()
            {
                Step = _dbContext.Steps.First(x => x.StepType == StepType.Offer),
                Internship = internship,
                PositionAscending = 2
            });
        
        _dbContext.SaveChanges();

    }

    public void SeedSteps()
    {
        if (_dbContext.Steps.Any())
        {
            return;
        }
        _dbContext.Steps.AddRange(
            new Step
            {
                Name = "Screening",
                StepType = StepType.Screening
            },
            new Step
            {
                Name = "Offer",
                StepType = StepType.Offer
            },
            new Step
            {
                Name = "Interview",
                StepType = StepType.Interview
            },
            new Step
            {
                Name = "Rejection",
                StepType = StepType.Rejection
            },
            new Step
            {
                Name = "Quiz",
                StepType = StepType.Quiz
            });
        _dbContext.SaveChanges();
    }
}