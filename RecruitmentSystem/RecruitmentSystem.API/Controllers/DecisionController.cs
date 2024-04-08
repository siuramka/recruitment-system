using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Constants;
using RecruitmentSystem.Domain.Dtos.Decision;
using RecruitmentSystem.Domain.Dtos.FinalScore;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API.Controllers;

public class DecisionController : ControllerBase
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;
    private readonly IOpenAiService _openAiService;
    private readonly IEvaluationService _evaluationService;
    private readonly IApplicationService _applicationService;
    private readonly IAuthService _authService;

    public DecisionController(
        RecruitmentDbContext db,
        IMapper mapper,
        IOpenAiService openAiService,
        IEvaluationService evaluationService,
        IApplicationService applicationService,
        IAuthService authService)
    {
        _db = db;
        _mapper = mapper;
        _openAiService = openAiService;
        _evaluationService = evaluationService;
        _applicationService = applicationService;
        _authService = authService;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/applications/{applicationId}/decision")]
    public async Task<IActionResult> GetDecision(Guid applicationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var authorized = await _authService.AuthorizeApplicationCompany(applicationId, userId);
        if (!authorized) return Forbid();

        var application = await _db.Applications
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));

        if (application is null)
        {
            return NotFound("Application not found");
        }

        var decision = await _db.Decisions.FirstOrDefaultAsync(d => d.ApplicationId == applicationId);

        if (decision is null)
        {
            return NotFound("Decision not found");
        }

        var finalScore = await _db.FinalScores.FirstOrDefaultAsync(fs => fs.ApplicationId == applicationId);

        var decisionScoreDto = new DecisionScoreDto
        {
            Decision = _mapper.Map<DecisionDto>(decision),
            FinalScore = _mapper.Map<FinalScoreDto>(finalScore),
        };

        return Ok(decisionScoreDto);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/applications/{applicationId}/decision")]
    public async Task<IActionResult> CreateDecision(Guid applicationId, [FromBody] DecisionCreateDto decisionCreateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var authorized = await _authService.AuthorizeApplicationCompany(applicationId, userId);
        if (!authorized) return Forbid();

        var application = await _db.Applications
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));

        if (application is null)
        {
            return NotFound("Application not found");
        }

        await _applicationService.EndApplication(application);

        var decision = new Decision
        {
            ApplicationId = applicationId,
            CompanySummary = decisionCreateDto.CompanySummary,
            CompanyStagesScores = decisionCreateDto.CompanyScore,
        };

        await _db.Decisions.AddAsync(decision);
        await _db.SaveChangesAsync();

        var decisionPrompt = await _openAiService.GenerateDecisionPrompt(applicationId);
        var decisionTask = _openAiService.GetFinalDecision(decisionPrompt);

        var candidateReviewTaskPrompt = await _openAiService.GenerateScreeningPrompt(applicationId);
        var candidateReviewTask = _openAiService.GetFitReview(candidateReviewTaskPrompt);

        await Task.WhenAll(decisionTask, candidateReviewTask);

        if (decisionTask.Result == null || candidateReviewTask.Result == null)
        {
            return BadRequest("AI service failed to return a result");
        }

        await _evaluationService.UpdateDecisionWithAiReview(decisionTask.Result, decision);
        await _evaluationService.UpdateDecisionWithFitnessReview(candidateReviewTask.Result, decision);

        var finalScore = await _evaluationService.CalculateFinalScore(applicationId);
        finalScore.ApplicationId = applicationId;

        _db.FinalScores.Add(finalScore);
        await _db.SaveChangesAsync();

        var decisionDto = _mapper.Map<DecisionDto>(decision);
        return CreatedAtAction(nameof(CreateDecision), decisionDto);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/applications/{applicationId}/decision/offer")]
    public async Task<IActionResult> DecisionHire(Guid applicationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var authorized = await _authService.AuthorizeApplicationCompany(applicationId, userId);
        if (!authorized) return Forbid();

        var application = await _db.Applications.FirstOrDefaultAsync(app => app.Id == applicationId);

        if (application is null)
            return NotFound("Application not found!");

        var internshipSteps =
            await _db.InternshipSteps
                .Include(internshipStep => internshipStep.Internship)
                .Include(internshipStep => internshipStep.Step)
                .Where(internshipStep => internshipStep.InternshipId == application.InternshipId)
                .ToListAsync();

        var nextStep = internshipSteps
            .FirstOrDefault(internshipStep => internshipStep.Step.StepType == StepType.Offer);

        application.InternshipStep = nextStep;

        _db.Update(application);
        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/applications/{applicationId}/decision/reject")]
    public async Task<IActionResult> DecisionReject(Guid applicationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var authorized = await _authService.AuthorizeApplicationCompany(applicationId, userId);
        if (!authorized) return Forbid();

        var application = await _db.Applications.FirstOrDefaultAsync(app => app.Id == applicationId);

        if (application is null)
            return NotFound("Application not found!");

        var internshipSteps =
            await _db.InternshipSteps
                .Include(internshipStep => internshipStep.Internship)
                .Include(internshipStep => internshipStep.Step)
                .Where(internshipStep => internshipStep.InternshipId == application.InternshipId)
                .ToListAsync();

        var nextStep = internshipSteps
            .FirstOrDefault(internshipStep => internshipStep.Step.StepType == StepType.Rejection);

        application.InternshipStep = nextStep;

        _db.Update(application);
        await _db.SaveChangesAsync();

        return Ok();
    }
}