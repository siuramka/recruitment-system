using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Dtos.Internship;

namespace RecruitmentSystem.API.Controllers;

[ApiController]
[Route("/api/internships/")]
public class InternshipController : ControllerBase
{
    private RecruitmentDbContext _db;
    private readonly IMapper _mapper;

    public InternshipController(RecruitmentDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetMany()
    {
        var internships = _db.Internships
            .Include(i => i.Company)
            .Select(i => _mapper.Map<InternshipDto>(i));

        return Ok(internships);
    }
}