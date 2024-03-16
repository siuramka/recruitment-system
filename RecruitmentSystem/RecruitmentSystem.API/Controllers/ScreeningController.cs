using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Evaluation;
using RecruitmentSystem.Domain.Dtos.Screening;
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
    private readonly OpenAiService _openAiService;
    private readonly ScoreService _scoreService;

    public ScreeningController(RecruitmentDbContext db, IMapper mapper, UserManager<SiteUser> userManager,
        PdfService pdfService, OpenAiService openAiService, ScoreService scoreService)
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
    [Route("/api/applications/{applicationId:guid}/screening")]
    public async Task<IActionResult> Get(Guid applicationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId!);

        var application = await _db.Applications
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));

        if (application is null)
            return BadRequest("Application not found");

        var cv = await _db.Cvs
            .FirstOrDefaultAsync(c => c.ApplicationId == application.Id);

        if (cv is null)
        {
            return NotFound("Cv not found");
        }
        //check if cv is null


        var cvDto = _mapper.Map<CvDto>(cv);
        return Ok(cvDto);
    }
    
    [HttpGet]
    [Authorize]
    [Route("/api/applications/{applicationId:guid}/skills")]
    public async Task<IActionResult> GetSkills(Guid applicationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId);

        var application = await _db.Applications
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));

        if (application is null)
            return BadRequest("Application not found");

        var cv = await _db.Cvs
            .FirstOrDefaultAsync(c => c.ApplicationId == application.Id);

        if (cv is null)
        {
            return NotFound("Cv not found");
        }

        var pdfString = _pdfService.GetTextFromPdf(cv.FileContent);

        return Ok();
    }

    [HttpGet]
    [Authorize]
    [Route("/api/applications/{applicationId:guid}/screening/cv")]
    public async Task<IActionResult> GetCv(Guid applicationId)
    {
        var application = await _db.Applications
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));

        if (application is null)
            return BadRequest("Application not found");

        var cv = await _db.Cvs
            .FirstOrDefaultAsync(c => c.ApplicationId == application.Id);

        if (cv is null)
        {
            return NotFound("Cv not found");
        }

        return File(cv.FileContent, "application/pdf", cv.FileName + ".pdf");
    }

    [HttpPost]
    [Authorize]
    [Route("/api/applications/{applicationId:guid}/screening")]
    public async Task<IActionResult> CreateScreening(IFormFile cvFile, Guid applicationId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var siteUser = await _userManager.FindByIdAsync(userId);

        var application = await _db.Applications
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));

        var internship = await _db.Internships.FirstOrDefaultAsync(i => i.Id.Equals(application.InternshipId));

        if (application is null)
            return NotFound("Application not found");

        var cv = await _db.Cvs
            .FirstOrDefaultAsync(c => c.ApplicationId.Equals(applicationId));

        if (cv is not null)
        {
            return Conflict("Cv already created");
        }
        
        if (cvFile.Length == 0)
        {
            return Conflict("Invalid file");
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
        
        var response = await _openAiService.GetScreeningScore(application.Id);
        
        if (response == null)
        {
            return BadRequest("Failed to fetch OPENAI");
        }
        
        await _scoreService.UpdateScreeningAiScore(application, response.fitnessScore);

        return CreatedAtAction(nameof(CreateScreening), new { cv.Id });
    }

    private static async Task<byte[]> GetPdfByteArray(IFormFile cvFile)
    {
        using var memoryStream = new MemoryStream();
        await cvFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}