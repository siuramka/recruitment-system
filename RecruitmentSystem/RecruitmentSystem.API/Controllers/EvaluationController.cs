using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    private readonly PdfService _pdfService;
    private readonly OpenAiService _openAiService;
    private readonly ScoreService _scoreService;

    public EvaluationController(RecruitmentDbContext db, IMapper mapper, UserManager<SiteUser> userManager, PdfService pdfService, OpenAiService openAiService, ScoreService scoreService)
    {
        _db = db;
        _mapper = mapper;
        _userManager = userManager;
        _pdfService = pdfService;
        _openAiService = openAiService;
        _scoreService = scoreService;
    }
    
    [HttpGet]
    [Authorize]
    [Route("/api/screening/{screeningId}/evaluation")]
    public async Task<IActionResult> GetScreeningEvaluation(Guid screeningId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId!);

        var cv = await _db.Cvs
            .FirstOrDefaultAsync(c => c.Id == screeningId);
        
        if (cv is null)
        {
            return NotFound("Cv not found");
        }

        var evaluation = await _db.Evaluations.FirstOrDefaultAsync(e => e.Id == cv.EvaluationId);

        if (evaluation is null)
        {
            return NotFound("Evaluation not found");
        }
        
        var evaluationDto = _mapper.Map<EvaluationDto>(evaluation);
        
        return Ok(evaluationDto);
    }
    
    [HttpPost]
    [Authorize]
    [Route("/api/screening/{screeningId}/evaluation")]
    public async Task<IActionResult> CreateCompanyScreeningEvaluation(Guid screeningId, [FromBody] EvaluationCreateDto evaluationCreateDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId!);

        var cv = await _db.Cvs
            .FirstOrDefaultAsync(c => c.Id == screeningId);
        
        if (cv is null)
        {
            return NotFound("Cv not found");
        }

        var evaluation = await _db.Evaluations.FirstOrDefaultAsync(e => e.Id == cv.EvaluationId);

        if (evaluation is null)
        {
            return NotFound("Evaluation not found");
        }

        _mapper.Map(evaluationCreateDto, evaluation);

        _db.Evaluations.Update(evaluation);
        await _db.SaveChangesAsync();
        
        var evaluationDto = _mapper.Map<EvaluationDto>(evaluation);
        return Ok(evaluationDto);
    }
    
    [HttpGet]
    [Authorize]
    [Route("/api/interview/{interviewId}/evaluation")]
    public async Task<IActionResult> GetInterviewEvaluation(Guid interviewId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId!);

        var interview = await _db.Interviews
            .FirstOrDefaultAsync(c => c.Id == interviewId);
        
        if (interview is null)
        {
            return NotFound("Cv not found");
        }

        var evaluation = await _db.Evaluations.FirstOrDefaultAsync(e => e.Id == interview.EvaluationId);

        if (evaluation is null)
        {
            return NotFound("Evaluation not found");
        }
        
        var evaluationDto = _mapper.Map<EvaluationDto>(evaluation);
        
        return Ok(evaluationDto);
    }
    
    [HttpGet]
    [Authorize]
    [Route("/api/assessment/{assessmentId}/evaluation")]
    public async Task<IActionResult> GetAssessment(Guid assessmentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId!);

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
        
        var evaluationDto = _mapper.Map<EvaluationDto>(evaluation);
        
        return Ok(evaluationDto);
    }
}