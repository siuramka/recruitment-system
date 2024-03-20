using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Decision;
using RecruitmentSystem.Domain.Dtos.Evaluation;
using RecruitmentSystem.Domain.Dtos.OpenAi;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Services;

public class EvaluationService
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;
    private readonly UserManager<SiteUser> _userManager;
    private readonly PdfService _pdfService;
    private readonly OpenAiService _openAiService;

    public EvaluationService(RecruitmentDbContext db, IMapper mapper, UserManager<SiteUser> userManager,
        PdfService pdfService, OpenAiService openAiService)
    {
        _db = db;
        _mapper = mapper;
        _userManager = userManager;
        _pdfService = pdfService;
        _openAiService = openAiService;
    }

    private static async Task<string> GetEvaluationStepName(Evaluation evaluation)
    {
        if (evaluation.Cv != null)
        {
            return StepType.Screening.ToString();
        }

        if (evaluation.Assessment != null)
        {
            return StepType.Assessment.ToString();
        }

        if (evaluation.Interview != null)
        {
            return StepType.Interview.ToString();
        }

        return "";
    }

    private async Task<List<Evaluation>> GetApplicationEvaluations(Guid applicationId)
    {
        return await _db.Evaluations
            .Include(e => e.Cv)
            .Include(e => e.Assessment)
            .Include(e => e.Interview)
            .Where(e => e.ApplicationId.Equals(applicationId))
            .ToListAsync();
    }

    private async Task<List<StepEvaluation>> GetStepEvaluations(Guid applicationId)
    {
        var evaluations = await GetApplicationEvaluations(applicationId);

        var stepEvaluations = new List<StepEvaluation>();

        foreach (var evaluation in evaluations)
        {
            var stepEvaluation = new StepEvaluation
            {
                StepName = await GetEvaluationStepName(evaluation),
                AiScoreForCandidateInStep = evaluation.AiScore,
                CompanyScoreForCandidateInStep = evaluation.CompanyScore,
                CompanysReviewOfCandidateInStep = evaluation.Content
            };

            stepEvaluations.Add(stepEvaluation);
        }

        return stepEvaluations;
    }

    private async Task<Decision?> GetFinalDecision(Guid applicationId)
    {
        return await _db.Decisions.FirstOrDefaultAsync(d => d.ApplicationId == applicationId);
    }

    public async Task<int> CalculateFinalScore(Guid applicationId)
    {
        var evaluations = await GetStepEvaluations(applicationId);
        var finalDecision = await GetFinalDecision(applicationId);
        var finalDecisonScore = finalDecision.AiStagesScore;

        var aiScoreWeight = int.Parse((await GetSettingByName(SettingsName.AiScoreWeight))!.Value);
        var companyScoreWeight =  int.Parse((await GetSettingByName(SettingsName.CompanyScoreWeight))!.Value);
        var totalScoreWeight =  int.Parse((await GetSettingByName(SettingsName.TotalScoreWeight))!.Value);

        var totalAiScore = evaluations.Sum(e => e.AiScoreForCandidateInStep);
        totalAiScore += finalDecisonScore;
        
        var totalCompanyScore = evaluations.Sum(e => e.CompanyScoreForCandidateInStep);

        var timeCoefficient = await GetApplicationAverageTimeCoefficient(applicationId);

        var finalScore =
            (aiScoreWeight * totalAiScore + companyScoreWeight * totalCompanyScore +
             totalScoreWeight * timeCoefficient) / (aiScoreWeight + companyScoreWeight + totalScoreWeight);
        
        return (int)MapToScale(finalScore, 0, 100, 1, 5);;
    }

    private double MapToScale(double value, double fromMin, double fromMax, double toMin, double toMax)
    {
        return (value - fromMin) * (toMax - toMin) / (fromMax - fromMin) + toMin;
    }

    private async Task<List<Application>> GetOfferedApplications()
    {
        return await _db.Applications
            .Include(a => a.InternshipStep)
            .ThenInclude(istep => istep.Step)
           // .Where(a => a.InternshipStep.Step.StepType == StepType.Offer)
            .ToListAsync();
    }

    private static async Task<TimeSpan> GetAverageTimeSpan(List<Application> applications)
    {
        return TimeSpan.FromTicks((long)applications.Average(x => (x.EndTime - x.CreatedOn).Ticks));
    }

    private async Task<double> GetApplicationAverageTimeCoefficient(Guid applicationId)
    {
        var applications = await GetOfferedApplications();
        var application = await _db.Applications.FirstOrDefaultAsync(app => app.Id == applicationId);

        if (application == null || applications.Count == 0) return 0;

        var averageTimeSpan = await GetAverageTimeSpan(applications);
        var applicationTimeSpan = application.EndTime - application.CreatedOn;

        return CalculateScore(applicationTimeSpan, averageTimeSpan);
    }

    private static double CalculateScore(TimeSpan applicationTimeSpan, TimeSpan averageTimeSpan)
    {
        const double percentileThreshold = 0.1;

        var applicationDays = applicationTimeSpan.TotalDays;
        var averageDays = averageTimeSpan.TotalDays;

        // Calculate the percentile of the application time compared to the average time
        var percentile = applicationDays / averageDays;

        // If application time is closer to the average than the given threshold, give a better score
        if (percentile <= 1 + percentileThreshold && percentile >= 1 - percentileThreshold)
        {
            // Normalize score between 0 and 100 based on the percentile
            return (1 - Math.Abs(1 - percentile)) * 100;
        }

        // If the application time is greater than the average, give score based on the difference
        if (applicationDays > averageDays)
        {
            return averageDays / applicationDays * 100;
        }

        return 0;
    }


    private async Task<Setting?> GetSettingByName(SettingsName name)
    {
        return await _db.Settings.FirstOrDefaultAsync(s => s.Name.Equals(name));
    }

    public async Task EvaluateInterviewAiScore(Guid interviewId)
    {
        var interview = await _db.Interviews
            .FirstOrDefaultAsync(c => c.Id == interviewId);

        var scoreResponse = await _openAiService.GetInterviewScore(interviewId);
        var evaluation = await _db.Evaluations.FirstOrDefaultAsync(e => e.Id == interview.EvaluationId);
        await UpdateEvaluationAiScore(evaluation, scoreResponse.interviewScore);
    }

    public async Task EvaluateAssessmentAiScore(Guid assessmentId)
    {
        var assessment = await _db.Assessments
            .FirstOrDefaultAsync(c => c.Id == assessmentId);

        var scoreResponse = await _openAiService.GetAssessmentScore(assessmentId);
        var evaluation = await _db.Evaluations.FirstOrDefaultAsync(e => e.Id == assessment.EvaluationId);

        await UpdateEvaluationAiScore(evaluation, scoreResponse.assessmentScore);
    }

    public async Task UpdateEvaluationAiScore(Evaluation evaluation, int score)
    {
        evaluation.AiScore = score;
        _db.Update(evaluation);
        await _db.SaveChangesAsync();
    }

    public async Task CreateEvaluationWithAiScore(Application application, int score)
    {
        var cv = await _db.Cvs.FirstOrDefaultAsync(cv => cv.ApplicationId == application.Id);

        var evaluation = new Evaluation()
        {
            AiScore = score,
            Content = string.Empty,
            ApplicationId = application.Id
        };

        _db.Evaluations.Add(evaluation);
        await _db.SaveChangesAsync();

        cv.EvaluationId = evaluation.Id;

        await _db.SaveChangesAsync();
    }

    public async Task<Evaluation> CreateEvaluation(EvaluationCreateDto evaluationCreateDto, Guid applicationId)
    {
        var evaluation = new Evaluation();
        _mapper.Map(evaluationCreateDto, evaluation);

        evaluation.ApplicationId = applicationId;

        _db.Evaluations.Add(evaluation);
        await _db.SaveChangesAsync();

        return evaluation;
    }

    public async Task AssignAssessmentEvaluation(Guid assessmentId, Guid evaluationId)
    {
        var assessment = await _db.Assessments
            .FirstOrDefaultAsync(c => c.Id == assessmentId);

        assessment.EvaluationId = evaluationId;

        _db.Assessments.Update(assessment);
        await _db.SaveChangesAsync();
    }

    public async Task AssignInterviewEvaluation(Guid interviewId, Guid evaluationId)
    {
        var interview = await _db.Interviews
            .FirstOrDefaultAsync(c => c.Id == interviewId);

        interview.EvaluationId = evaluationId;

        _db.Interviews.Update(interview);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateScreeningCompanyEvaluation(Guid? evaluationId, EvaluationCreateDto evaluationCreateDto)
    {
        var evaluation = await _db.Evaluations.FirstOrDefaultAsync(e => e.Id == evaluationId);
        _mapper.Map(evaluationCreateDto, evaluation);
        _db.Evaluations.Update(evaluation);

        await _db.SaveChangesAsync();
    }

    public async Task UpdateDecisionWithAiReview(DecisionResponse decisionResponse, Decision decision)
    {
        decision.AiStagesReview = decisionResponse.stagesReview;
        decision.AiStagesScore = decisionResponse.finalDecision;

        _db.Update(decision);
        await _db.SaveChangesAsync();
    }
}