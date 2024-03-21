using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Domain.Constants;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.DataAccess.Seeders;

public class DataSeeder
{
    private RecruitmentDbContext _dbContext;
    private readonly UserManager<SiteUser> _userManager;
    private RoleManager<IdentityRole> _roleManager;

    public DataSeeder(RecruitmentDbContext dbContext, UserManager<SiteUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
    }

    public async Task Seed()
    {
        await SeedRoles();
        await SeedCompany();
        await SeedUser();
        await SeedSteps();
        await SeedInternship();
    }

    private async Task SeedInternship()
    {
        var containsAny = await _dbContext.Internships.ToListAsync();

        if (containsAny.Count != 0)
        {
            return;
        }

        await _dbContext.SaveChangesAsync();

        var company = await _dbContext.Companys.FirstAsync();
        await _dbContext.Internships.AddAsync(new Internship()
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
        await _dbContext.SaveChangesAsync();

        var internship = await _dbContext.Internships.FirstAsync();

        await _dbContext.InternshipSteps.AddRangeAsync(
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

        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedSteps()
    {
        var containsAny = await _dbContext.Steps.ToListAsync();

        if (containsAny.Count != 0)
        {
            return;
        }

        await _dbContext.Steps.AddRangeAsync(
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
                Name = "Evaluation",
                StepType = StepType.Decision
            },
            new Step
            {
                Name = "Rejection",
                StepType = StepType.Rejection
            },
            new Step
            {
                Name = "Assessment",
                StepType = StepType.Assessment
            });
        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedCompany()
    {
        var siteUser = new SiteUser()
        {
            Email = "company@x.com",
            UserName = "company@x.com",
            FirstName = "Compotas",
            LastName = "Compotenauskis",
            DateOfBirth = new DateTime(1999, 01, 12).ToUniversalTime(),
            Location = "Kaunas"
        };

        var company = new Company()
        {
            Email = "hr@company.com",
            Name = "Mariusoftas",
            Location = "Kaunas",
            PhoneNumber = "+370613221321",
            Website = "company.com"
        };

        var existingAdminUser = await _userManager.FindByEmailAsync(siteUser.Email);

        if (existingAdminUser == null)
        {
            var createAdminUserResult = await _userManager.CreateAsync(siteUser, "VerySafePassword1!");
            if (createAdminUserResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(siteUser, Roles.Company);
            }

            var user = await _dbContext.SiteUsers.FirstOrDefaultAsync(x => x.Email == siteUser.Email);
            user.Company = company;

            await _dbContext.SaveChangesAsync();
        }
    }

    private async Task SeedUser()
    {
        var siteUser = new SiteUser()
        {
            Email = "user@x.com",
            UserName = "user@x.com",
            FirstName = "Useronas",
            LastName = "Lastonas",
            DateOfBirth = new DateTime(2000, 11, 12).ToUniversalTime(),
        };

        var existingAdminUser = await _userManager.FindByEmailAsync(siteUser.Email);

        if (existingAdminUser == null)
        {
            var createAdminUserResult = await _userManager.CreateAsync(siteUser, "VerySafePassword1!");
            if (createAdminUserResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(siteUser, Roles.SiteUser);
            }
        }
    }

    public async Task SeedRoles()
    {
        foreach (var role in Roles.SiteRoles)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
                await _roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}