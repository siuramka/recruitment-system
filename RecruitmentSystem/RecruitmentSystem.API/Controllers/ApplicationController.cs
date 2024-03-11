using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public ApplicationController(RecruitmentDbContext db, IMapper mapper, UserManager<SiteUser> userManager)
    {
        _db = db;
        _mapper = mapper;
        _userManager = userManager;
    }
    
    [HttpGet]
    [Authorize]
    [Route("/api/applications/{applicationId:guid}")]
    public async Task<IActionResult> GetById(Guid applicationId)
    {
        var application = await _db.Applications
            .Include(app => app.Internship)
            .ThenInclude(i => i.Company)
            .Include(app => app.SiteUser)
            .Include(app => app.InternshipStep)
            .ThenInclude(istep => istep.Step)
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));
        
        if (application is null)
            return NotFound("Application not found!");
        
        return Ok(_mapper.Map<ApplicationListItemDto>(application));
    }
    
    [HttpGet]
    [Authorize(Roles = Roles.SiteUser)]
    [Route("/api/applications")]
    public async Task<IActionResult> GetAllUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId);
        
        var applications = await _db.Applications
            .Include(app => app.Internship)
            .ThenInclude(i => i.Company)
            .Include(app => app.SiteUser)
            .Include(app => app.InternshipStep)
            .ThenInclude(istep => istep.Step)
            .Where(app => app.SiteUser == siteUser)
            .ToListAsync();

        return Ok(applications.Select(dto => _mapper.Map<ApplicationListItemDto>(dto)));
    }
    
    [HttpGet]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/internships/{internshipId:guid}/applications")]
    public async Task<IActionResult> GetAll(Guid internshipId)
    {
        var internship = await _db.Internships
            .Include(i => i.InternshipSteps)
            .FirstOrDefaultAsync(internship => internship.Id == internshipId);

        if (internship is null)
        {
            return NotFound("Internship not found");
        }
        
        var internshipApplications = await _db.Applications
            .Where(app => app.InternshipId == internshipId)
            .Include(app => app.Internship)
            .ThenInclude(i => i.Company)
            .Include(app => app.SiteUser)
            .Include(app => app.InternshipStep)
            .ThenInclude(istep => istep.Step)
            .ToListAsync();

        return Ok(internshipApplications.Select(dto => _mapper.Map<ApplicationListItemDto>(dto)));
    }
    
    
    [HttpPost]
    [Authorize]
    [Route("/api/internships/{internshipId:guid}/applications")]
    public async Task<IActionResult> Create(Guid internshipId)
    {
        var internship = await _db.Internships
            .Include(i => i.InternshipSteps)
            .FirstOrDefaultAsync(internship => internship.Id == internshipId);

        if (internship is null)
        {
            return NotFound("Internship not found");
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId);
        
        var existingApplication = await _db.Applications
            .Include(ap => ap.Internship)
            .Include(ap => ap.SiteUser)
            .FirstOrDefaultAsync(ap => ap.SiteUser.Id == siteUser.Id 
                                       && ap.Internship.Id == internshipId );

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

        _db.Applications.Add(application);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Create), _mapper.Map<ApplicationDto>(application));
    }
}