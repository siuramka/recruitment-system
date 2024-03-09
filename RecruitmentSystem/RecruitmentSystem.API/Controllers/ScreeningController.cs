using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;

namespace RecruitmentSystem.API.Controllers;

[ApiController]
public class ScreeningController : ControllerBase
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;
    private readonly UserManager<SiteUser> _userManager;
    private readonly PdfService _pdfService;

    public ScreeningController(RecruitmentDbContext db, IMapper mapper, UserManager<SiteUser> userManager,
        PdfService pdfService)
    {
        _db = db;
        _mapper = mapper;
        _userManager = userManager;
        _pdfService = pdfService;
    }
    
    
    [HttpGet]
    [Authorize]
    [Route("/api/internships/{internshipId:guid}/application/screening")]
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
                                       && ap.Internship.Id == internshipId);

        if (application is null)
            return BadRequest("Application not found");

        var cv = await _db.Cvs
            .FirstOrDefaultAsync(c => c.InternshipId == internshipId && c.ApplicationId == application.Id);

        if (cv is null)
        {
            return NotFound("Cv not found");
        }

        return Ok();
    }

    [HttpGet]
    [Authorize]
    [Route("/api/internships/{internshipId:guid}/application/screening/cv")]
    public async Task<IActionResult> GetCv(Guid internshipId)
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
                                       && ap.Internship.Id == internshipId);

        if (application is null)
            return BadRequest("Application not found");

        var cv = await _db.Cvs
            .FirstOrDefaultAsync(c => c.InternshipId == internshipId && c.ApplicationId == application.Id);

        if (cv is null)
        {
            return NotFound("Cv not found");
        }

        return File(cv.FileContent, "application/pdf", cv.FileName + ".pdf");
    }

    [HttpPost]
    [Authorize]
    [Route("/api/internships/{internshipId:guid}/application/screening")]
    public async Task<IActionResult> CreateScreening(IFormFile cvFile, Guid internshipId)
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
                                       && ap.Internship.Id == internshipId);

        if (application is null)
            return BadRequest("Application not found");

        var cv = await _db.Cvs
            .FirstOrDefaultAsync(c => c.InternshipId == internshipId && c.ApplicationId == application.Id);

        if (cv is not null)
        {
            return NotFound("Cv already created!");
        }
        
        if (cvFile.Length == 0)
        {
            return BadRequest("Invalid file");
        }

        var pdfBytes = await GetPdfByteArray(cvFile);

        cv = new Cv
        {
            Internship = internship,
            Application = application,
            SiteUser = siteUser,
            FileName = siteUser.FirstName + "-" + siteUser.LastName + "-" + DateTime.Now.Date,
            FileContent = pdfBytes
        };

        _db.Cvs.Add(cv);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateScreening), new { cv.Id });
    }

    private static async Task<byte[]> GetPdfByteArray(IFormFile cvFile)
    {
        using var memoryStream = new MemoryStream();
        await cvFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}