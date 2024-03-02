using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Domain.Constants;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.DataAccess.Seeders;

public class AuthSeeder
{
    private readonly UserManager<SiteUser> _userManager;
    private RecruitmentDbContext _dbContext;
    private RoleManager<IdentityRole> _roleManager;

    public AuthSeeder(RecruitmentDbContext dbContext, RoleManager<IdentityRole> roleManager,
        UserManager<SiteUser> userManager)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    private async Task SeedCompany()
    {
        var siteUser = new SiteUser()
        {
            Email = "company@x.com",
            UserName = "company@x.com",
            FirstName = "Compotas",
            LastName = "Compotenauskis",
            DateOfBirth = new DateTime(1999,01,12).ToUniversalTime(),
            Location = "Kaunas"
        };

        var company = new Company
        {
            Name = "teltonika",
            Email = "hr@teltonika.com",
            Location = "Kaunas",
            PhoneNumber = "+123213123",
            Website = "www.teltonika.lt"
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
            DateOfBirth = new DateTime(2000,11,12).ToUniversalTime(),
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
        await SeedCompany();
        await SeedUser();
        
        foreach (var role in Roles.SiteRoles)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
                await _roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}