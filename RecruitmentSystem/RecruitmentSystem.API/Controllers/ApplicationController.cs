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
    [Authorize(Roles = Roles.SiteUser)]
    [Route("/api/internships/{internshipId:guid}/application")]
    public async Task<IActionResult> Get(Guid internshipId)
    {
        var internship = await _db.Internships.Include(i => i.InternshipSteps)
            .FirstOrDefaultAsync(internship => internship.Id == internshipId);

        if (internship is null)
        {
            return NotFound("Internship not found!");
        }
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId);
        
        var application = await _db.Applications
            .FirstOrDefaultAsync(ap => ap.SiteUser.Id == siteUser.Id 
                            && ap.Internship.Id == internshipId );
        
        if (application is null)
            return NotFound("Application not found!");
        
        return Ok(new ApplicationDto { Id = application.Id});
    }

    [HttpPost]
    [Authorize(Roles = Roles.SiteUser)]
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