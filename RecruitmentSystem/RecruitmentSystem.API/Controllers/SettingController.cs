using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Constants;
using RecruitmentSystem.Domain.Dtos.Setting;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API.Controllers;

public class SettingController : ControllerBase
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;
    private readonly UserManager<SiteUser> _userManager;
    private readonly PdfService _pdfService;
    private readonly OpenAiService _openAiService;
    private readonly AssessmentService _assessmentService;
    
    public SettingController(RecruitmentDbContext db, IMapper mapper, UserManager<SiteUser> userManager,
        PdfService pdfService, OpenAiService openAiService, AssessmentService assessmentService)
    {
        _db = db;
        _mapper = mapper;
        _userManager = userManager;
        _pdfService = pdfService;
        _openAiService = openAiService;
        _assessmentService = assessmentService;
    }
    
    [HttpGet]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/settings")]
    public async Task<IActionResult> GetAll()
    {
        var settings = await _db.Settings.ToListAsync();
        return Ok(_mapper.Map<List<SettingDto>>(settings));
    }
    
    [HttpPut]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/settings")]
    public async Task<IActionResult> Update([FromQuery] SettingsName settingsName, [FromBody] SettingCreateDto settingCreateDto)
    {
        var setting = await _db.Settings.FirstOrDefaultAsync(s => s.Name == settingsName);
        
        if (setting is null)
        {
            return NotFound("Setting not found");
        }
        
        setting.Value = settingCreateDto.Value;
        await _db.SaveChangesAsync();

        return Ok(_mapper.Map<SettingDto>(setting));
    }
}