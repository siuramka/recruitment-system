using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Constants;
using RecruitmentSystem.Domain.Dtos.Application;
using RecruitmentSystem.Domain.Dtos.Steps;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API.Controllers;

[ApiController]
public class StepsController : ControllerBase
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;
    private IStepsService _stepsService;
    private IApplicationService _applicationService;
    
    public StepsController(RecruitmentDbContext db, IMapper mapper, IStepsService stepsService,
        IApplicationService applicationService)
    {
        _db = db;
        _mapper = mapper;
        _stepsService = stepsService;
        _applicationService = applicationService;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/steps")]
    public async Task<IActionResult> GetAvailableSteps()
    {
        var steps = await _db.Steps.ToListAsync();
        return Ok(steps.Select(_mapper.Map<StepDto>));
    }

    [HttpGet]
    [Authorize]
    [Route("/api/application/{applicationId}/steps")]
    public async Task<IActionResult> Get(Guid applicationId)
    {
        var application = await _applicationService.GetApplicaitonById(applicationId);
        
        if (application is null)
            return NotFound("Application not found!");

        var internshipSteps =
            await _stepsService.GetInternshipSteps(application.InternshipId);

        var currentStep = application.InternshipStep;

        var internshipStepDtos = internshipSteps
            .OrderBy(internshipStep => internshipStep.PositionAscending)
            .Select(internshipStep => new ApplicationStepDto
            {
                StepType = internshipStep.Step.StepType.ToString(),
                PositionAscending = internshipStep.PositionAscending,
                IsCurrentStep = internshipStep.Step.Id == currentStep.Step.Id
            });

        return Ok(internshipStepDtos);
    }

    [HttpGet]
    [Authorize]
    [Route("/api/internships/{internshipId:guid}/application/{applicationId:guid}/steps")]
    public async Task<IActionResult> GetByApplication(Guid internshipId, Guid applicationId)
    {
        var internship = await _db.Internships
            .FirstOrDefaultAsync(internship => internship.Id == internshipId);

        if (internship is null)
            return NotFound("Internship not found!");

        var internshipSteps =
            await _stepsService.GetInternshipSteps(internshipId);

        var application = await _db.Applications.FirstOrDefaultAsync(app => app.Id == applicationId);

        if (application is null)
            return NotFound("Application not found!");

        var currentStep = application.InternshipStep;

        var internshipStepDtos = internshipSteps
            .OrderBy(internshipStep => internshipStep.PositionAscending)
            .Select(internshipStep => new ApplicationStepDto
            {
                StepType = internshipStep.Step.StepType.ToString(),
                PositionAscending = internshipStep.PositionAscending,
                IsCurrentStep = internshipStep.Step.Id == currentStep.Step.Id
            });

        return Ok(internshipStepDtos);
    }

    [HttpPost]
    [Authorize]
    [Route("/api/internships/{internshipId:guid}/application/{applicationId:guid}/steps/next")]
    public async Task<IActionResult> NextApplicationStep(Guid internshipId, Guid applicationId)
    {
        var internship = await _db.Internships
            .FirstOrDefaultAsync(internship => internship.Id == internshipId);

        if (internship is null)
            return NotFound("Internship not found!");

        var application = await _db.Applications.FirstOrDefaultAsync(app => app.Id == applicationId);

        if (application is null)
            return NotFound("Application not found!");

        var internshipSteps =
            await _stepsService.GetInternshipSteps(internshipId);

        var currentStep = application.InternshipStep;
        var nextStep = internshipSteps
            .FirstOrDefault(internshipStep => internshipStep.PositionAscending == currentStep.PositionAscending + 1);

        if (nextStep.Step.StepType is StepType.Offer or StepType.Rejection)
        {
            return BadRequest("Application process has already finished");
        }

        application.InternshipStep = nextStep;

        _db.Update(application);
        await _db.SaveChangesAsync();

        return Ok(new StepDto() { StepType = nextStep.Step.StepType.ToString() });
    }

    [HttpPut]
    [Authorize]
    [Route("/api/internships/{internshipId:guid}/application/{applicationId:guid}/steps")]
    public async Task<IActionResult> UpdateApplicationStep(Guid internshipId, Guid applicationId,
        [FromBody] UpdateApplicationStepDto updateApplicationStepDto)
    {
        var internship = await _db.Internships
            .FirstOrDefaultAsync(internship => internship.Id == internshipId);

        if (internship is null)
            return NotFound("Internship not found!");

        var newInternshipStep =
            await _stepsService.GetInternshpStepByType(internshipId, updateApplicationStepDto.StepType);

        if (newInternshipStep is null)
        {
            return NotFound("Application can't be updated to this step!");
        }

        var application = await _db.Applications.FirstOrDefaultAsync(app => app.Id == applicationId);

        if (application is null)
            return NotFound("Application not found!");

        application.InternshipStep = newInternshipStep;

        await _applicationService.UpdateApplication(application);

        return Ok();
    }
}