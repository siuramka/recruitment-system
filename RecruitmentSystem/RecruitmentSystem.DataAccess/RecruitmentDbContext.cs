using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.DataAccess;

public class RecruitmentDbContext : IdentityDbContext<SiteUser>
{
    public RecruitmentDbContext()
    {
    }
    
    public RecruitmentDbContext(DbContextOptions<RecruitmentDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Step>()
            .Property(a => a.StepType)
            .HasConversion<string>();
        
        modelBuilder.Entity<Application>()
            .Property(a => a.ScoreStatus)
            .HasConversion<string>();
        
        // modelBuilder.Entity<InternshipStep>()
        //     .HasKey(elo => new { elo.StepId, elo.InternshipId, elo.Id });

        base.OnModelCreating(modelBuilder);
    }

    public virtual DbSet<SiteUser> SiteUsers { get; set; }
    public virtual DbSet<Step> Steps { get; set; }
    public virtual DbSet<Application> Applications { get; set; }
    public virtual DbSet<Cv> Cvs { get; set; }
    public virtual DbSet<Company> Companys { get; set; }
    public virtual DbSet<Internship> Internships { get; set; }
    public virtual DbSet<InternshipStep> InternshipSteps { get; set; }
    public virtual DbSet<Interview> Interviews { get; set; }
    public virtual DbSet<Assessment> Assessments { get; set; }
    public virtual DbSet<Evaluation> Evaluations { get; set; }
    public virtual DbSet<Setting> Settings { get; set; }
    public virtual DbSet<Decision> Decisions { get; set; }
    public virtual DbSet<FinalScore> FinalScores { get; set; }

}