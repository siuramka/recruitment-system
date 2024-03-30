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

    public async Task<List<Evaluation>> GetApplicationEvaluations(Guid applicationId)
    {
        return await _db.Evaluations
            .Include(e => e.Cv)
            .Include(e => e.Assessment)
            .Include(e => e.Interview)
            .Where(e => e.ApplicationId.Equals(applicationId))
            .ToListAsync();
    }

    public async Task<List<StepEvaluation>> GetStepEvaluations(Guid applicationId)
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

    public async Task<Decision?> GetFinalDecision(Guid applicationId)
    {
        return await _db.Decisions.FirstOrDefaultAsync(d => d.ApplicationId == applicationId);
    }

    private static double CalculateCorrelation(int[] aiScores, int[] companyScores)
    {
        if (aiScores.Length != companyScores.Length)
            throw new Exception("Scores dont match");

        var aiArray = aiScores.ToArray();
        var companyArray = companyScores.ToArray();

        long n = aiScores.Length;
        long sumAi = 0;
        long sumCompany = 0;
        long sumAiSquared = 0;
        long sumCompanySquared = 0;
        long sumAiCompany = 0;

        Parallel.For(0, n, i =>
        {
            sumAi += aiArray[i];
            sumCompany += companyArray[i];
            sumAiSquared += aiArray[i] * aiArray[i];
            sumCompanySquared += companyArray[i] * companyArray[i];
            sumAiCompany += aiArray[i] * companyArray[i];
        });

        var correlationR = (n * sumAiCompany - sumAi * sumCompany) /
                           Math.Sqrt((n * sumAiSquared - sumAi * sumAi) *
                                     (n * sumCompanySquared - sumCompany * sumCompany));

        return correlationR;
    }

    private static int[] GetCompanyScores(List<StepEvaluation> stepEvaluations, Decision finalDecision)
    {
        var companyScores = new int[stepEvaluations.Count + 1];

        for (int i = 0; i < stepEvaluations.Count; i++)
        {
            companyScores[i] = (int)stepEvaluations[i].CompanyScoreForCandidateInStep;
        }

        companyScores[stepEvaluations.Count] = finalDecision.CompanyStagesScores;

        return companyScores;
    }

    private static int[] GetAiScores(List<StepEvaluation> stepEvaluations, Decision finalDecision)
    {
        var aiScores = new int[stepEvaluations.Count + 1];

        for (int i = 0; i < stepEvaluations.Count; i++)
        {
            aiScores[i] = (int)stepEvaluations[i].AiScoreForCandidateInStep;
        }

        aiScores[stepEvaluations.Count] = finalDecision.AiStagesScore;

        return aiScores;
    }

    public async Task CreateFinalScore(FinalScore finalScore)
    {

    }

    public async Task<FinalScore> CalculateFinalScore(Guid applicationId)
    {
        const int maxScore = 100;
        const double normalize = maxScore / 5;

        var evaluations = await GetStepEvaluations(applicationId);
        var finalDecision = await GetFinalDecision(applicationId);
        var weights = await GetApplicationInternshipSetting(applicationId);

        var companyScores = GetCompanyScores(evaluations, finalDecision);
        var aiScores = GetAiScores(evaluations, finalDecision);
        var correlation = CalculateCorrelation(aiScores, companyScores);

        var aiScoreX1 = aiScores.Average() * normalize * (weights.AiScoreWeight / maxScore);
        var companyScoreX2 = companyScores.Average() * normalize * (weights.CompanyScoreWeight / maxScore);

        var x1x2Average = (aiScoreX1 + companyScoreX2) / 2;

        var correlationBoostModifer = (1 + correlation) * (weights.TotalScoreWeight / maxScore);

        var finalScore = x1x2Average * correlationBoostModifer;

        var correlationBoostValue = finalScore - x1x2Average;
        
        if (finalScore > maxScore) finalScore = maxScore;

        return new FinalScore
        {
            Score = Math.Round(finalScore, 2),
            AiScoreX1 = Math.Round(aiScoreX1, 2),
            CompanyScoreX2 = Math.Round(companyScoreX2, 2),
            Correlation = Math.Round(correlation, 2),
            X1X2Average = Math.Round(x1x2Average, 2),
            CorrelationBoostValue = Math.Round(correlationBoostValue, 2),
            CorrelationBoostModifer = Math.Round(correlationBoostModifer, 2),
        };
    }

    private async Task<Setting> GetApplicationInternshipSetting(Guid applicaitonId)
    {
        var application = await _db.Applications.FindAsync(applicaitonId);
        return await _db.Settings.FirstOrDefaultAsync(s => s.InternshipId == application.InternshipId);
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