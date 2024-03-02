using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public ScreeningController(RecruitmentDbContext db, IMapper mapper, UserManager<SiteUser> userManager)
    {
        _db = db;
        _mapper = mapper;
        _userManager = userManager;
        
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
                                       && ap.Internship.Id == internshipId );

        if (application is null)
            return BadRequest("Application not found");
        
        if (cvFile.Length == 0)
        {
            return BadRequest("Invalid file");
        }
        
        byte[] pdfBytes;
        using (var memoryStream = new MemoryStream())
        {
            await cvFile.CopyToAsync(memoryStream);
            pdfBytes = memoryStream.ToArray();
        }

        var cv = new Cv
        {
            Internship = internship,
            Application = application,
            SiteUser = siteUser,
            FileName = siteUser.FirstName + "-" + siteUser.LastName + "-" + DateTime.Now.Date,
            FileContent = pdfBytes
        };

         _db.Cvs.Add(cv);
         await _db.SaveChangesAsync();
    
        PdfLoadedDocument loadedDocument = new PdfLoadedDocument(pdfBytes);
        PdfPageBase page = loadedDocument.Pages[0];
        string extractedTexts = page.ExtractText(true);
        loadedDocument.Close(true);
        //
        return Ok(extractedTexts);
    }

    
}