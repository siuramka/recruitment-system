namespace RecruitmentSystem.Domain.Dtos.Statistics;

public class CombinedStatisticsDto
{
    public string Step { get; set; }
    public double AiAverage { get; set; }
    public double CompanyAverage { get; set; }
    public double CandidateAiScore { get; set; }
    public double CandidateCompanyScore { get; set; }
}