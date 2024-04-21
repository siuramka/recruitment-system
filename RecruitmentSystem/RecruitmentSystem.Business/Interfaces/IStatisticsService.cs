using RecruitmentSystem.Domain.Dtos.Decision;
using RecruitmentSystem.Domain.Dtos.Statistics;

namespace RecruitmentSystem.Business.Interfaces;

public interface IStatisticsService
{
    Task<List<StepEvaluation>> GetEvaluationsAsync(Guid applicationId);
    Task<List<LineStatisticsDto>> GetApplicationLineChartDataAsync(Guid applicationId);
    Task<List<CombinedStatisticsDto>> GetApplicationCombinedChartDataAsync(Guid applicationId);
}