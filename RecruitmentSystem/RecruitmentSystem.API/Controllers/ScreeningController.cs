using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Models;

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
    
    
    // [HttpPost]
    // [Route("/api/internships/{internshipId:guid}/application/screening")]
    // public async Task<IActionResult> CreateScreening(IFormFile cvFile)
    // {
    //     if (cvFile.Length == 0)
    //     {
    //         return BadRequest("Invalid file");
    //     }
    //
    //     PdfLoadedDocument loadedDocument = new PdfLoadedDocument(fileName);
    //     PdfPageBase page = loadedDocument.Pages[0];
    //     string extractedTexts = page.ExtractText(true);
    //     loadedDocument.Close(true);
    //
    //     return Ok(pdfContent);
    // }

    
}