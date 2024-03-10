using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Constants;
using RecruitmentSystem.Domain.Dtos.Internship;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API.Controllers;

[ApiController]
public class InternshipController : ControllerBase
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;

    public InternshipController(RecruitmentDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/internships")]
    public async Task<IActionResult> Create(InternshipCreateDto internshipCreateDto)
    {
        var internship = _mapper.Map<Internship>(internshipCreateDto);
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var company = await _db.Companys
            .Include(c => c.SiteUser)
            .FirstOrDefaultAsync(c => c.SiteUser.Id == userId);

        if (company is null)
        {
            return BadRequest("Company not found");
        }

        internship.Company = company;
        internship.CreatedAt = DateTime.Now.ToUniversalTime();
        internship.StartDate = DateTime.Now.ToUniversalTime();
        internship.EndDate = DateTime.Now.ToUniversalTime();//TODO: DELETE THES UNECESSSARY FUCKED SHADCN DATE PICKER

        _db.Internships.Add(internship);
        await _db.SaveChangesAsync();

        List<InternshipStep> internshipSteps = new();

        foreach (var internshipStepDto in internshipCreateDto.InternshipStepDtos)
        {
            var stepType = Enum.Parse<StepType>(internshipStepDto.StepType);

            var step = await _db.Steps.FirstOrDefaultAsync(s => s.StepType == stepType);
            if (step is null)
            {
                return BadRequest("StepType not found");
            }

            var internshipStep = new InternshipStep
            {
                InternshipId = internship.Id, StepId = step.Id, PositionAscending = internshipStepDto.PositionAscending
            };

            internshipSteps.Add(internshipStep);
        }

        internship.InternshipSteps = internshipSteps;
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Create), _mapper.Map<InternshipDto>(internship));
    }

//todo: add picture for company
    [HttpGet]
    [Route("/api/internships")]
    [Authorize(Roles = Roles.SiteUser + ", " + Roles.Company)]
    public async Task<IActionResult> GetMany()
    {
        if (User.HasClaim(ClaimTypes.Role, Roles.SiteUser))
        {
            var internships = _db.Internships
                .Include(i => i.Company)
                .Select(i => _mapper.Map<InternshipDto>(i));

            return Ok(internships);
        }

        if (User.HasClaim(ClaimTypes.Role, Roles.Company))
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var company = await _db.Companys
                .Include(c => c.SiteUser)
                .FirstOrDefaultAsync(c => c.SiteUser.Id == userId);

            if (company is null)
            {
                return BadRequest("Company not found");
            }

            var internships = _db.Internships
                .Include(i => i.Company)
                .Where(internship => internship.CompanyId == company.Id)
                .Select(i => _mapper.Map<InternshipDto>(i));

            return Ok(internships);
        }

        return Forbid();
    }

    [HttpGet]
    [Route("/api/internships/{internshipId:guid}")]
    [Authorize(Roles = Roles.SiteUser + ", " + Roles.Company)]
    public async Task<IActionResult> GetOne(Guid internshipId)
    {
        var internship = await _db.Internships
            .Include(i => i.Company)
            .FirstOrDefaultAsync(internship => internship.Id == internshipId);

        if (internship is null)
            return NotFound("Internship not found!");

        var internshipDto = _mapper.Map<InternshipDto>(internship);

        return Ok(internshipDto);
    }
}