using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Constants;
using RecruitmentSystem.Domain.Dtos.Application;
using RecruitmentSystem.Domain.Dtos.Internship;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API.Controllers;

[ApiController]
public class StepsController : ControllerBase
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;
    private readonly UserManager<SiteUser> _userManager;

    public StepsController(RecruitmentDbContext db, IMapper mapper, UserManager<SiteUser> userManager)
    {
        _db = db;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize]
    [Route("/api/internships/{internshipId:guid}/application/steps")]
    public async Task<IActionResult> Get(Guid internshipId)
    {
        var internship = await _db.Internships
            .FirstOrDefaultAsync(internship => internship.Id == internshipId);
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId);

        if (internship is null)
            return NotFound("Internship not found!");

        var internshipSteps =
            await _db.InternshipSteps
                .Include(internshipStep => internshipStep.Internship)
                .Include(internshipStep => internshipStep.Step)
                .Where(internshipStep => internshipStep.InternshipId == internshipId)
                .ToListAsync();
        
        var application = await _db.Applications
            .Include(app => app.InternshipStep)
            .ThenInclude(internshipStep => internshipStep.Step)
            .FirstOrDefaultAsync(ap => ap.SiteUser.Id == siteUser.Id 
                                       && ap.Internship.Id == internshipId );

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

}