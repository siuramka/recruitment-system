namespace RecruitmentSystem.Domain.Models;

public class Evaluation
{
    public Guid Id { get; set; }

    public int AiScore { get; set; }
    public int CompanyScore { get; set; }
    public int Score { get; set; }
    public string Content { get; set; } = string.Empty;
    
    public Cv? Cv { get; set; }
    public Interview? Interview { get; set; }
    public Assessment? Assessment { get; set; }
}