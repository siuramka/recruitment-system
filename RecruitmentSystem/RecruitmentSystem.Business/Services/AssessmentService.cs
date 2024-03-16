using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Assessment;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.Business.Services;

public class AssessmentService
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;
    private readonly UserManager<SiteUser> _userManager;
    private readonly PdfService _pdfService;
    private readonly OpenAiService _openAiService;
    
    public AssessmentService(RecruitmentDbContext db, IMapper mapper, UserManager<SiteUser> userManager,
        PdfService pdfService, OpenAiService openAiService )
    {
        _db = db;
        _mapper = mapper;
        _userManager = userManager;
        _pdfService = pdfService;
        _openAiService = openAiService;
    }

    public async Task<Assessment> CreateAssessment(Guid applicationId, AssessmentCreateDto assessmentCreateDto)
    {
        var newAssessment = new Assessment
        {
            ApplicationId = applicationId,
            Content = assessmentCreateDto.Content,
            StartTime = DateTime.Now.ToUniversalTime(),
            EndTime = assessmentCreateDto.EndTime.ToUniversalTime()
        };
        
        await _db.Assessments.AddAsync(newAssessment);
        await _db.SaveChangesAsync();

        return newAssessment;
    }
}