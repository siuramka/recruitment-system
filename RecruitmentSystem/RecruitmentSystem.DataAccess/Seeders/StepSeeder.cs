using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.DataAccess.Seeders;

public class StepSeeder
{
    private RecruitmentDbContext _dbContext;

    public StepSeeder(RecruitmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedSteps()
    {
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