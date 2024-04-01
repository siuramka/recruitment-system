using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Assessment;

namespace RecruitmentSystem.API.Controllers;

public class AssessmentController : ControllerBase
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;
    private readonly IAssessmentService _assessmentService;
    
    public AssessmentController(RecruitmentDbContext db, IMapper mapper, IAssessmentService assessmentService)
    {
        _db = db;
        _mapper = mapper;
        _assessmentService = assessmentService;
    }
    
    [HttpGet]
    [Authorize]
    [Route("/api/applications/{applicationId:guid}/assessment")]
    public async Task<IActionResult> Get(Guid applicationId)
    {
        var application = await _db.Applications
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));

        if (application is null)
            return BadRequest("Application not found");

        var assessment = await _db.Assessments.FirstOrDefaultAsync(i => i.ApplicationId.Equals(applicationId));

        if (assessment is null)
        {
            return NotFound("Assessment not found");
        }

        return Ok(_mapper.Map<AssessmentDto>(assessment));
    }
    
    [HttpPost]
    [Authorize]
    [Route("/api/applications/{applicationId:guid}/assessment")]
    public async Task<IActionResult> CreateAssessment(Guid applicationId, [FromBody] AssessmentCreateDto assessmentCreateDto)
    {
        var application = await _db.Applications
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));

        if (application is null)
            return BadRequest("Application not found");

        var assessment = await _db.Assessments.FirstOrDefaultAsync(assessment => assessment.ApplicationId == applicationId);
        
        if(assessment is not null) 
            return BadRequest("Assessment already exists");

        var createdAssessment = await _assessmentService.CreateAssessment(applicationId, assessmentCreateDto);

        return Ok(_mapper.Map<AssessmentDto>(createdAssessment));
    }
}