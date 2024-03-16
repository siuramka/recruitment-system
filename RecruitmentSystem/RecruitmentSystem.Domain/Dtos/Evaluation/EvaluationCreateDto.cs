namespace RecruitmentSystem.Domain.Dtos.Evaluation;

public class EvaluationCreateDto
{
    public int CompanyScore { get; set; }
    public string Content { get; set; } = string.Empty;
}