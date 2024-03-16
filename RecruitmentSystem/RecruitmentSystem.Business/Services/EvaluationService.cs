using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Evaluation;
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
    public async Task UpdateScreeningAiScore(Application application, int score)
    {
        var cv = await _db.Cvs.FirstOrDefaultAsync(cv => cv.ApplicationId == application.Id);
        
        var evaluation = new Evaluation()
        {
            AiScore = score,
            Content = string.Empty,
        };

        _db.Evaluations.Add(evaluation);
        await _db.SaveChangesAsync();

        cv.EvaluationId = evaluation.Id;

        await _db.SaveChangesAsync();
    }

    public async Task<Evaluation> CreateEvaluation(EvaluationCreateDto evaluationCreateDto)
    {
        var evaluation = new Evaluation();
        _mapper.Map(evaluationCreateDto, evaluation);

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
}