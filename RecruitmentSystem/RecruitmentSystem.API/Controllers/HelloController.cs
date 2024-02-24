using Microsoft.AspNetCore.Mvc;

namespace RecruitmentSystem.API.Controllers;

[ApiController]
[Route("/hello")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public IActionResult GetHello()
    {
        return Ok("hello");
    }
}