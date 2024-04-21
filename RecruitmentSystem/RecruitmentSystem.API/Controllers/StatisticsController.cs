using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentSystem.Business.Interfaces;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.Domain.Constants;

namespace RecruitmentSystem.API.Controllers;

public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticService;
    private readonly IAuthService _authService;

    public StatisticsController(
        IStatisticsService statisticService,
        IAuthService authService)
    {
        _statisticService = statisticService;
        _authService = authService;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/applications/{applicationId:guid}/statistics/line")]
    public async Task<IActionResult> GetLine(Guid applicationId)
    {
        var authorized = await _authService
            .AuthorizeApplicationCreatorOrCompany(applicationId, User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (!authorized) return Forbid();
        
        var statistics = await _statisticService.GetApplicationLineChartDataAsync(applicationId);

        if (statistics.Count == 0)
        {
            return BadRequest();
        }

        return Ok(statistics);
    }
    
    [HttpGet]
    [Authorize(Roles = Roles.Company)]
    [Route("/api/applications/{applicationId:guid}/statistics/combined")]
    public async Task<IActionResult> GetCombined(Guid applicationId)
    {
        var authorized = await _authService
            .AuthorizeApplicationCreatorOrCompany(applicationId, User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (!authorized) return Forbid();
        
        var statistics = await _statisticService.GetApplicationCombinedChartDataAsync(applicationId);

        if (statistics.Count == 0)
        {
            return BadRequest();
        }

        return Ok(statistics);
    }
}