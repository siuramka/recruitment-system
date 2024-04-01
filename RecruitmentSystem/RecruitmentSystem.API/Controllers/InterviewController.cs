using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Interview;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API.Controllers;

public class InterviewController : ControllerBase
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;
    public InterviewController(RecruitmentDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Authorize]
    [Route("/api/applications/{applicationId:guid}/interview")]
    public async Task<IActionResult> Get(Guid applicationId)
    {
        var application = await _db.Applications
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));

        if (application is null)
            return BadRequest("Application not found");

        var interview = await _db.Interviews.FirstOrDefaultAsync(i => i.ApplicationId.Equals(applicationId));

        if (interview is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<InterviewDto>(interview));
    }
    [HttpPost]
    [Authorize]
    [Route("/api/applications/{applicationId:guid}/interview")]
    public async Task<IActionResult> ScheduleInterview(Guid applicationId, [FromBody] InterviewCreateDto interviewCreateDto)
    {
        var application = await _db.Applications
            .FirstOrDefaultAsync(ap => ap.Id.Equals(applicationId));

        if (application is null)
            return BadRequest("Application not found");

        var interview = await _db.Interviews.FirstOrDefaultAsync(i => i.ApplicationId.Equals(applicationId));
        
        if (interview is not null)
        {
            return BadRequest("Interview already created");
        }
        
        var newInterview = new Interview()
        {
            ApplicationId = applicationId,
            StartTime = interviewCreateDto.StartTime.ToUniversalTime(),
            MinutesLength = interviewCreateDto.MinutesLength,
            Instructions = interviewCreateDto.Instructions
        };
            
        await _db.Interviews.AddAsync(newInterview);
        await _db.SaveChangesAsync();

            
        return Ok(_mapper.Map<InterviewDto>(newInterview));
    }
}