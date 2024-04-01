using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecruitmentSystem.Business.Services;
using RecruitmentSystem.DataAccess;
using RecruitmentSystem.Domain.Constants;
using RecruitmentSystem.Domain.Dtos.Auth;
using RecruitmentSystem.Domain.Models;

namespace RecruitmentSystem.API.Controllers;

[ApiController]
[Route("/api/auth/")]
public class AuthController : ControllerBase
{
    private readonly UserManager<SiteUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _configuration;
    private readonly RecruitmentDbContext _db;

    public AuthController(UserManager<SiteUser> userManager, IJwtService jwtService, IConfiguration configuration,
        RecruitmentDbContext db)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _configuration = configuration;
        _db = db;
    }

    [HttpPost]
    [Route("register/user")]
    public async Task<ActionResult> RegisterUser(RegisterUserDto registerUserDto)
    {
        var userWithEmail = await _userManager.FindByEmailAsync(registerUserDto.Email);
        if (userWithEmail != null)
        {
            return BadRequest();
        }

        SiteUser newUser = new SiteUser
        {
            Email = registerUserDto.Email,
            UserName = registerUserDto.Email,
            PhoneNumber = registerUserDto.PhoneNumber,
            Location = registerUserDto.Location,
            DateOfBirth = registerUserDto.DateOfBirth.ToUniversalTime(),
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName
        };

        var createUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);
        if (!createUserResult.Succeeded)
        {
            return BadRequest("Could not create a user.");
        }

        await _userManager.AddToRoleAsync(newUser, Roles.SiteUser);

        return Ok();
    }

    [HttpPost]
    [Route("register/company")]
    public async Task<ActionResult> RegisterCompany(RegisterCompanyDto registerCompanyDto)
    {
        var userWithEmail = await _userManager.FindByEmailAsync(registerCompanyDto.RegisterUserDto.Email);
        if (userWithEmail != null)
        {
            return BadRequest();
        }

        Company company = new Company
        {
            Email = registerCompanyDto.Email,
            Name = registerCompanyDto.Name,
            PhoneNumber = registerCompanyDto.PhoneNumber,
            Location = registerCompanyDto.Location,
            Website = registerCompanyDto.Website
        };

        await _db.Companys.AddAsync(company);
        await _db.SaveChangesAsync();

        SiteUser newUser = new SiteUser
        {
            Email = registerCompanyDto.RegisterUserDto.Email,
            UserName = registerCompanyDto.RegisterUserDto.Email,
            PhoneNumber = registerCompanyDto.RegisterUserDto.PhoneNumber,
            Location = registerCompanyDto.RegisterUserDto.Location,
            DateOfBirth = registerCompanyDto.RegisterUserDto.DateOfBirth.ToUniversalTime(),
            FirstName = registerCompanyDto.RegisterUserDto.FirstName,
            LastName = registerCompanyDto.RegisterUserDto.LastName,
            Company = company
        };

        var createUserResult = await _userManager.CreateAsync(newUser, registerCompanyDto.RegisterUserDto.Password);
        if (!createUserResult.Succeeded)
        {
            return BadRequest("Could not create a user.");
        }

        await _userManager.AddToRoleAsync(newUser, Roles.Company);
        return Ok();
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return BadRequest("Email or password is invalid.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordValid)
        {
            return BadRequest("Email or password is invalid");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtService.CreateAccessToken(user.UserName!, user.Id, roles);

        await UpdateUsersRefreshTokenWithExpiration(user);

        return Ok(new SuccessfullLoginDto(accessToken, user.RefreshToken!));
    }

    
    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenDto refreshTokenDto)
    {
        ClaimsPrincipal? principal;
    
        try
        {
            principal = _jwtService.GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken);
        }
        catch
        {
            return BadRequest("Invalid token");
        }
    
        var userName = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(userName);
        
        var isRefreshTokenInvalid = user!.RefreshToken != refreshTokenDto.RefreshToken ||
                                    user.RefreshTokenExpiryTime <= DateTime.UtcNow;
    
        if (user == null || isRefreshTokenInvalid)
        {
            return BadRequest();
        }
    
        var roles = await _userManager.GetRolesAsync(user);
        var newAccessToken = _jwtService.CreateAccessToken(user.UserName!, user.Id, roles);
    
        user.RefreshToken = _jwtService.CreateRefreshToken();
    
        await _userManager.UpdateAsync(user);
    
        return Ok(new SuccessfullLoginDto(newAccessToken, user.RefreshToken));
    }

    [Authorize]
    [HttpPost]
    [Route("revoke")]
    public async Task<IActionResult> Revoke()
    {
        var user = await _userManager.GetUserAsync(User);
    
        if (user == null || user.RefreshToken == null)
            return BadRequest();
    
        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = DateTime.MinValue.ToUniversalTime();
    
        await _userManager.UpdateAsync(user);
    
        return Ok();
    }

    private async Task UpdateUsersRefreshTokenWithExpiration(SiteUser user)
    {
        var refreshTokenValidityDays = Int32.Parse(_configuration["JWT:RefreshTokenValidityDays"]);
        user.RefreshToken = _jwtService.CreateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityDays);
        
        await _userManager.UpdateAsync(user);
    }
}