using System.Text.Json.Serialization;

namespace RecruitmentSystem.Domain.Dtos.Statistics;

public class LineStatisticsDto
{
    [JsonPropertyName("name")]
    public string StepName { get; set; }
    [JsonPropertyName("ai")]
    public int AiScoreForCandidateInStep { get; set; }
    [JsonPropertyName("company")]
    public int CompanyScoreForCandidateInStep { get; set; }
}