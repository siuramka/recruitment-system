using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<SiteUser> _userManager;
    private readonly PdfService _pdfService;
    private readonly OpenAiService _openAiService;
    private readonly EvaluationService _evaluationService;
    private readonly ApplicationService _applicationService;

    public DecisionController(RecruitmentDbContext db, IMapper mapper, UserManager<SiteUser> userManager,
        PdfService pdfService, OpenAiService openAiService,
        EvaluationService evaluationService, ApplicationService applicationService)
    {
        _db = db;
        _mapper = mapper;
        _userManager = userManager;
        _pdfService = pdfService;
        _openAiService = openAiService;
        _evaluationService = evaluationService;
        _applicationService = applicationService;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/applications/{applicationId}/decision")]
    public async Task<IActionResult> GetDecision(Guid applicationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId!);

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
        var siteUser = await _userManager.FindByIdAsync(userId!);

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
        };

        await _db.Decisions.AddAsync(decision);
        await _db.SaveChangesAsync();

        var decisionResponse = await _openAiService.GetFinalDecision(applicationId);
        await _evaluationService.UpdateDecisionWithAiReview(decisionResponse, decision);

        var finalScore = await _evaluationService.CalculateFinalScore(applicationId);
        finalScore.ApplicationId = applicationId;

        await _evaluationService.CreateFinalScore(finalScore);

        var decisionDto = _mapper.Map<DecisionDto>(decision);
        return CreatedAtAction(nameof(CreateDecision), decisionDto);
    }
}