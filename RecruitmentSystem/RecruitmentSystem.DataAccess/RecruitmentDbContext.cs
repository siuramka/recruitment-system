using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.DataAccess;

public class RecruitmentDbContext : IdentityDbContext<SiteUser>
{
    public RecruitmentDbContext(DbContextOptions<RecruitmentDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Step>()
            .Property(a => a.StepType)
            .HasConversion<string>();

        // modelBuilder.Entity<InternshipStep>()
        //     .HasKey(elo => new { elo.StepId, elo.InternshipId, elo.Id });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<SiteUser> SiteUsers { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<Cv> Cvs { get; set; }
    public DbSet<Company> Companys { get; set; }
    public DbSet<Internship> Internships { get; set; }
    public DbSet<InternshipStep> InternshipSteps { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    public DbSet<Assessment> Assessments { get; set; }
}