using RecruitmentSystem.Domain.Dtos.FinalScore;

namespace RecruitmentSystem.Domain.Dtos.Decision;

public class DecisionScoreDto
{
    public DecisionDto Decision { get; set; }
    public FinalScoreDto FinalScore { get; set; }
}