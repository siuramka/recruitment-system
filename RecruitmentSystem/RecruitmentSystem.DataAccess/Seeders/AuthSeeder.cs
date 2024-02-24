using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RecruitmentSystem.Domain.Constants;

namespace RecruitmentSystem.DataAccess.Seeders;

public class AuthSeeder
{
    private RecruitmentDbContext _dbContext;
    private  RoleManager<IdentityRole> _roleManager;

    public AuthSeeder(RecruitmentDbContext dbContext, RoleManager<IdentityRole> roleManager)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
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