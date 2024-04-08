using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.Domain.Constants;
using RecruitmentSystem.Domain.Dtos.Internship;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API.Controllers;

[ApiController]
public class InternshipController : ControllerBase
{
    private readonly IMapper _mapper;
    private IInternshipService _internshipService;

    public InternshipController(
        IMapper mapper,
        IInternshipService internshipService)
    {
        _mapper = mapper;
        _internshipService = internshipService;
    }

    [HttpPost]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/internships")]
    public async Task<IActionResult> Create(InternshipCreateDto internshipCreateDto)
    {
        Internship internship;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        try
        {
            internship = await _internshipService.CreateInternshipAsync(internshipCreateDto, userId);
        }
        catch
        {
            return BadRequest("Failed to create internship!");
        }

        return CreatedAtAction(nameof(Create), _mapper.Map<InternshipDto>(internship));
    }

    [HttpGet]
    [Route("/api/internships")]
    [Authorize(Roles = Roles.SiteUser + ", " + Roles.Company)]
    public async Task<IActionResult> GetMany()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (User.HasClaim(ClaimTypes.Role, Roles.SiteUser))
        {
            var internships = await _internshipService.GetAllInternshipsAsDtoAsync();
            return Ok(internships);
        }

        if (User.HasClaim(ClaimTypes.Role, Roles.Company))
        {
            var internships = await _internshipService.GetAllInternshipsAsDtoOfCompanyAsync(userId);
            return Ok(internships);
        }

        return Forbid();
    }

    [HttpGet]
    [Route("/api/internships/{internshipId:guid}")]
    [Authorize(Roles = Roles.SiteUser + ", " + Roles.Company)]
    public async Task<IActionResult> GetOne(Guid internshipId)
    {
        var internship = await _internshipService.GetInternshipByIdIncludeCompany(internshipId);

        if (internship is null)
            return NotFound("Internship not found!");

        var internshipDto = _mapper.Map<InternshipDto>(internship);

        return Ok(internshipDto);
    }
}