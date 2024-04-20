using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Decision;
using RecruitmentSystem.Domain.Dtos.Statistics;
namespace RecruitmentSystem.Business.Services;

public interface IStatisticsService
{
    Task<List<StepEvaluation>> GetEvaluationsAsync(Guid applicationId);
    Task<List<LineStatisticsDto>> GetApplicationLineChartDataAsync(Guid applicationId);
    Task<List<CombinedStatisticsDto>> GetApplicationCombinedChartDataAsync(Guid applicationId);
}

public class StatisticsService : IStatisticsService
{
    private RecruitmentDbContext _db;
    private IEvaluationService _evaluationService;
    private readonly IMapper _mapper;

    public StatisticsService(IMapper mapper, RecruitmentDbContext db,
        IEvaluationService evaluationService)
    {
        _mapper = mapper;
        _db = db;
        _evaluationService = evaluationService;
    }

    public async Task<List<StepEvaluation>> GetEvaluationsAsync(Guid applicationId)
    {
        var evaluations = await _evaluationService.GetStepEvaluations(applicationId);

        var finalDecision = await _evaluationService.GetFinalDecision(applicationId);
        
        evaluations.Add(new StepEvaluation
        {
            StepName = "Final Decision",
            AiScoreForCandidateInStep = finalDecision.AiStagesScore,
            CompanyScoreForCandidateInStep = finalDecision.CompanyStagesScores,
        });

        return evaluations;
    }

    public async Task<List<LineStatisticsDto>> GetApplicationLineChartDataAsync(Guid applicationId)
    {
        var application = _db.Applications.Find(applicationId);
        
        if(application.EndTime == default)
        {
            return null;
        }

        var evaluations = await GetEvaluationsAsync(applicationId);
        
        return _mapper.Map<List<LineStatisticsDto>>(evaluations);
    }
    
    public async Task<List<CombinedStatisticsDto>> GetApplicationCombinedChartDataAsync(Guid applicationId)
    {
        var application = await _db.Applications.FindAsync(applicationId);

        var applications = await _db.Applications.Where(a => a.InternshipId == application.InternshipId).ToListAsync();
        
        var evaluations = new List<StepEvaluation>();
        
        foreach (var app in applications)
        {
            var stepEvaluations = await _evaluationService.GetStepEvaluations(app.Id);
            evaluations.AddRange(stepEvaluations);
            
            var finalDecision = await _evaluationService.GetFinalDecision(applicationId);
            evaluations.Add(new StepEvaluation
            {
                StepName = "Final Decision",
                AiScoreForCandidateInStep = finalDecision.AiStagesScore,
                CompanyScoreForCandidateInStep = finalDecision.CompanyStagesScores,
            });
        }
        
        var candidateFinalDecision = await _evaluationService.GetFinalDecision(applicationId);
        evaluations.Add(new StepEvaluation
        {
            StepName = "Final Decision",
            AiScoreForCandidateInStep = candidateFinalDecision.AiStagesScore,
            CompanyScoreForCandidateInStep = candidateFinalDecision.CompanyStagesScores,
        });

        var interviewEvaluations = evaluations
            .GroupBy(e => e.StepName)
            .Select(group => new StepEvaluation
        {
            StepName = group.Key,
            AiScoreForCandidateInStep = group.Average(e => e.AiScoreForCandidateInStep),
            CompanyScoreForCandidateInStep = group.Average(e => e.CompanyScoreForCandidateInStep),
        }).ToList();
        
        var applicationEvaluations = await GetEvaluationsAsync(applicationId);
        var combinedStatisticsDtos = CombineStepEvaluationsAsync(interviewEvaluations, applicationEvaluations);
        
        return combinedStatisticsDtos;
    }

    private static List<CombinedStatisticsDto> CombineStepEvaluationsAsync(List<StepEvaluation> interviewEvaluations,
        List<StepEvaluation> applicationEvaluations)
    {
        var combinedStatisticsDtos = new List<CombinedStatisticsDto>();
        
        foreach (var applicationEvaluation in applicationEvaluations)
        {
            var interviewEvaluation = interviewEvaluations.FirstOrDefault(e => e.StepName == applicationEvaluation.StepName);
            combinedStatisticsDtos.Add(new CombinedStatisticsDto
            {
                Step = applicationEvaluation.StepName,
                AiAverage = interviewEvaluation?.AiScoreForCandidateInStep ?? 0,
                CompanyAverage = interviewEvaluation?.CompanyScoreForCandidateInStep ?? 0,
                CandidateAiScore = applicationEvaluation.AiScoreForCandidateInStep,
                CandidateCompanyScore = applicationEvaluation.CompanyScoreForCandidateInStep,
            });
        }
        
        return combinedStatisticsDtos;
    }
    
    
}