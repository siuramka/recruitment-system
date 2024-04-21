using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Business.Interfaces;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Constants;
using RecruitmentSystem.Domain.Dtos.Application;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API.Controllers;

[ApiController]
public class ApplicationController : ControllerBase
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;
    private readonly UserManager<SiteUser> _userManager;
    private readonly IApplicationService _applicationService;
    private readonly IAuthService _authService;

    public ApplicationController(
        RecruitmentDbContext db,
        IMapper mapper,
        UserManager<SiteUser> userManager,
        IAuthService authService,
        IApplicationService applicationService)
    {
        _db = db;
        _mapper = mapper;
        _userManager = userManager;
        _authService = authService;
        _applicationService = applicationService;
    }
    
    [HttpGet]
    [Authorize]
    [Route("/api/applications/{applicationId:guid}")]
    public async Task<IActionResult> GetById(Guid applicationId)
    {
        var authorized = await _authService
            .AuthorizeApplicationCreatorOrCompany(applicationId, User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        if (!authorized) return Forbid();

        var application = await _applicationService.GetApplicaitonById(applicationId);
        
        if (application is null)
            return NotFound("Application not found!");
        
        return Ok(_mapper.Map<ApplicationListItemDto>(application));
    }
    
    [HttpGet]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/applications/decisions")]
    public async Task<IActionResult> GetAllCompanyDecisions()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId);

        var applications = await _applicationService.GetDecisionApplications(siteUser.CompanyId);

        return Ok(applications.Select(dto => _mapper.Map<ApplicationListItemDto>(dto)));
    }
    
    [HttpGet]
    [Authorize(Roles = Roles.SiteUser)]
    [Route("/api/applications")]
    public async Task<IActionResult> GetAllUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId);
        
        var applications = await _applicationService.GetUserApplications(siteUser);

        return Ok(applications.Select(dto => _mapper.Map<ApplicationListItemDto>(dto)));
    }
    
    [HttpGet]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/internships/{internshipId:guid}/applications")]
    public async Task<IActionResult> GetAll(Guid internshipId)
    {
        var authorized = await _authService
            .AuthorizeInternshipCompany(internshipId, User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        if (!authorized) return Forbid();
        
        var internship = await _db.Internships
            .Include(i => i.InternshipSteps)
            .FirstOrDefaultAsync(internship => internship.Id == internshipId);

        if (internship is null)
        {
            return NotFound("Internship not found");
        }
        
        var internshipApplications = await _applicationService.GetInternshipApplications(internshipId);

        return Ok(internshipApplications.Select(dto => _mapper.Map<ApplicationListItemDto>(dto)));
    }
    
    [HttpPost]
    [Authorize(Roles = Roles.SiteUser)]
    [Route("/api/internships/{internshipId:guid}/applications")]
    public async Task<IActionResult> Create(Guid internshipId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var internship = await _db.Internships
            .Include(i => i.InternshipSteps)
            .FirstOrDefaultAsync(internship => internship.Id == internshipId);

        if (internship is null)
        {
            return NotFound("Internship not found");
        }
        
        var siteUser = await _userManager.FindByIdAsync(userId);

        var existingApplication = await _applicationService.IsApplicationCreated(internshipId, userId);

        if (existingApplication is not null)
            return BadRequest("Already applied!");
        
        var application = new Application
        {
            CreatedOn = DateTime.Now.ToUniversalTime(),
            Internship = internship,
            InternshipStep = internship.InternshipSteps.OrderBy(x => x.PositionAscending).First(),
            SiteUser = siteUser,
            Skills = ""
        };

        await _applicationService.CreateApplication(application);

        return CreatedAtAction(nameof(Create), _mapper.Map<ApplicationDto>(application));
    }
}