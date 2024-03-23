using RecruitmentSystem.Domain.Dtos.Statistics;

namespace RecruitmentSystem.Business.Services.Interfaces;

public interface IStatisticService
{
    Task<List<LineStatisticsDto>> GetApplicationLineChartDataAsync(Guid applicationId);
    Task<List<CombinedStatisticsDto>> GetApplicationCombinedChartDataAsync(Guid applicationId);
}