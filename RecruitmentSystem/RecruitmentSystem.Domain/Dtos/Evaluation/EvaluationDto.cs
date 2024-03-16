namespace RecruitmentSystem.Domain.Dtos.Evaluation;

public class EvaluationDto
{
    public Guid Id { get; set; }
    public int AiScore { get; set; }
    public int CompanyScore { get; set; }
    public int Score { get; set; }
    public string Content { get; set; }
}