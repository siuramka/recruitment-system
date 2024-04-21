using System.Security.Claims;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using RecruitmentSystem.Business.Services;

namespace RecruitmentSystem.Tests.Services;

public class JwtServiceTests
{
    private Mock<IConfiguration> _configuration;
    
    [SetUp]
    public void SetUp()
    {
        _configuration = new Mock<IConfiguration>();
    }

    [Test]
    public async Task CreateRefreshToken_ShouldCreateRefreshToken()
    {
        _configuration.Setup(c => c["JWT:Secret"]).Returns(new string('a', 256));
        _configuration.Setup(c => c["JWT:ValidIssuer"]).Returns(new string('a', 256));
        _configuration.Setup(c => c["JWT:ValidAudience"]).Returns(new string('a', 256));
        
        var jwtService = new JwtService(_configuration.Object);
        
        var result = jwtService.CreateRefreshToken();
        
        result.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task GetPrincipalFromExpiredToken_ShouldGetPrincipalFromExpiredToken()
    {
        _configuration.Setup(c => c["JWT:Secret"]).Returns(new string('a', 256));
        _configuration.Setup(c => c["JWT:ValidIssuer"]).Returns(new string('a', 256));
        _configuration.Setup(c => c["JWT:ValidAudience"]).Returns(new string('a', 256));
        
        var jwtService = new JwtService(_configuration.Object);

        var token = jwtService.CreateAccessToken("TestName", "1", new List<string> { "Admin" });
        var principal = jwtService.GetPrincipalFromExpiredToken(token);

        var claimsIdentity = principal.Identity as ClaimsIdentity;
        
        token.Should().NotBeNullOrEmpty();
        claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value.Should().Be("1");
        claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value.Should().Be("TestName");
        claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value.Should().Be("Admin");
    }

}