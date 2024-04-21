using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Business.Interfaces;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Evaluation;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API.Controllers;

public class EvaluationController : ControllerBase
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;
    private readonly UserManager<SiteUser> _userManager;
    private readonly IEvaluationService _evaluationService;
    private IAuthService _authService;

    public EvaluationController(
        RecruitmentDbContext db,
        IMapper mapper,
        UserManager<SiteUser> userManager,
        IEvaluationService evaluationService,
        IAuthService authService)
    {
        _db = db;
        _mapper = mapper;
        _userManager = userManager;
        _evaluationService = evaluationService;
        _authService = authService;
    }


    [HttpGet]
    [Authorize]
    [Route("/api/screening/{screeningId}/evaluation")]
    public async Task<IActionResult> GetScreeningEvaluation(Guid screeningId)
    {
        var cv = await _db.Cvs
            .FirstOrDefaultAsync(c => c.Id == screeningId);

        if (cv is null)
        {
            return NotFound("Cv not found");
        }

        var evaluation = await _db.Evaluations.FirstOrDefaultAsync(e => e.Id == cv.EvaluationId);

        if (evaluation is null)
        {
            return Conflict("Evaluation not found");
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var authorized = await _authService.AuthorizeEvaluationCompany(evaluation.Id, userId);
        if (!authorized) return Forbid();

        var evaluationDto = _mapper.Map<EvaluationDto>(evaluation);

        return Ok(evaluationDto);
    }

    [HttpPost]
    [Authorize]
    [Route("/api/screening/{screeningId}/evaluation")]
    public async Task<IActionResult> CreateCompanyScreeningEvaluation(Guid screeningId,
        [FromBody] EvaluationCreateDto evaluationCreateDto)
    {
        var cv = await _db.Cvs.FirstOrDefaultAsync(c => c.Id == screeningId);

        if (cv is null)
        {
            return NotFound("Cv not found");
        }

        var evaluation = await _db.Evaluations.FirstOrDefaultAsync(e => e.Id == cv.EvaluationId);

        if (evaluation is null)
        {
            return NotFound("Evaluation not found");
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var applicationId = cv.ApplicationId;
        var authorized = await _authService.AuthorizeApplicationCompany(applicationId, userId);
        if (!authorized) return Forbid();

        await _evaluationService.UpdateScreeningCompanyEvaluation(cv.EvaluationId, evaluationCreateDto);

        var evaluationDto = _mapper.Map<EvaluationDto>(evaluation);
        return Ok(evaluationDto);
    }

    [HttpGet]
    [Authorize]
    [Route("/api/interview/{interviewId}/evaluation")]
    public async Task<IActionResult> GetInterviewEvaluation(Guid interviewId)
    {
        var interview = await _db.Interviews
            .FirstOrDefaultAsync(c => c.Id == interviewId);

        if (interview is null)
        {
            return NotFound("Interview not found");
        }

        var evaluation = await _db.Evaluations.FirstOrDefaultAsync(e => e.Id == interview.EvaluationId);

        if (evaluation is null)
        {
            return NotFound("Evaluation not found");
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var authorized = await _authService.AuthorizeEvaluationCompany(evaluation.Id, userId);
        if (!authorized) return Forbid();

        var evaluationDto = _mapper.Map<EvaluationDto>(evaluation);
        return Ok(evaluationDto);
    }

    [HttpPost]
    [Authorize]
    [Route("/api/interview/{interviewId}/evaluation")]
    public async Task<IActionResult> CreateInterviewEvaluation(Guid interviewId,
        [FromBody] EvaluationCreateDto evaluationCreateDto)
    {
        var interview = await _db.Interviews
            .FirstOrDefaultAsync(c => c.Id == interviewId);

        if (interview is null)
        {
            return NotFound("Interview not found");
        }

        var applicationId = interview.ApplicationId;

        var evaluation = _db.Evaluations.FirstOrDefault(e => e.Id == interview.EvaluationId);
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var authorized = await _authService.AuthorizeApplicationCompany(applicationId, userId);
        if (!authorized) return Forbid();

        if (evaluation is not null)
        {
            return NotFound("Evaluation already exists");
        }
        
        var createdEvaluation = await _evaluationService.CreateEvaluation(evaluationCreateDto, applicationId);
        
        await _evaluationService.AssignInterviewEvaluation(interviewId, createdEvaluation.Id);
        
        await _evaluationService.EvaluateInterviewAiScore(interviewId);

        var evaluationDto = _mapper.Map<EvaluationDto>(createdEvaluation);
        return Ok(evaluationDto);
    }

    [HttpPost]
    [Authorize]
    [Route("/api/assessment/{assessmentId}/evaluation")]
    public async Task<IActionResult> CreateAssessmentEvaluation(Guid assessmentId,
        [FromBody] EvaluationCreateDto evaluationCreateDto)
    {
        var assessment = await _db.Assessments
            .FirstOrDefaultAsync(c => c.Id == assessmentId);

        if (assessment is null)
        {
            return NotFound("Assessment not found");
        }

        var applicationId = assessment.ApplicationId;

        var evaluation = _db.Evaluations.FirstOrDefault(e => e.Id == assessment.EvaluationId);

        if (evaluation is not null)
        {
            return NotFound("Evaluation already exists");
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var authorized = await _authService.AuthorizeApplicationCompany(applicationId, userId);
        if (!authorized) return Forbid();

        var createdEvaluation = await _evaluationService.CreateEvaluation(evaluationCreateDto, applicationId);
        await _evaluationService.AssignAssessmentEvaluation(assessmentId, createdEvaluation.Id);
        await _evaluationService.EvaluateAssessmentAiScore(assessmentId);

        var evaluationDto = _mapper.Map<EvaluationDto>(createdEvaluation);
        return Ok(evaluationDto);
    }

    [HttpGet]
    [Authorize]
    [Route("/api/assessment/{assessmentId}/evaluation")]
    public async Task<IActionResult> GetAssessmentEvaluation(Guid assessmentId)
    {
        var assessment = await _db.Assessments
            .FirstOrDefaultAsync(c => c.Id == assessmentId);

        if (assessment is null)
        {
            return NotFound("Assessment not found");
        }

        var evaluation = await _db.Evaluations.FirstOrDefaultAsync(e => e.Id == assessment.EvaluationId);

        if (evaluation is null)
        {
            return NotFound("Evaluation not found");
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var authorized = await _authService.AuthorizeEvaluationCompany(evaluation.Id, userId);
        if (!authorized) return Forbid();

        var evaluationDto = _mapper.Map<EvaluationDto>(evaluation);

        return Ok(evaluationDto);
    }
}