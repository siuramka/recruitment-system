using System.ComponentModel.DataAnnotations;

namespace RecruitmentSystem.Domain.Models;

public class Cv
{
    public Guid Id { get; set; }
    
    public Guid InternshipId { get; set; }
    public Internship Internship { get; set; }
    
    public Guid ApplicationId { get; set; }
    public Application Application { get; set; }
    
    public SiteUser SiteUser { get; set; }
    
    public string FileName { get; set; }
    
    public byte[] FileContent { get; set; }
    
    public Guid? EvaluationId { get; set; }
}