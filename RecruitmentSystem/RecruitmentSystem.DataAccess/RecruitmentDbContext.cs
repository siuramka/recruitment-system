using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.DataAccess;

public class RecruitmentDbContext : DbContext
{
    public RecruitmentDbContext(DbContextOptions<RecruitmentDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Step>()
            .Property(a => a.StepType)
            .HasConversion<string>();

        modelBuilder.Entity<InternshipStep>()
            .HasKey(elo => new { elo.StepId, elo.InternshipId });

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<SiteUser> SiteUsers { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<Cv> Cvs { get; set; }
    public DbSet<Anwser> Anwsers { get; set; }
    public DbSet<Company> Companys { get; set; }
}