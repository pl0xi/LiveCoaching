using Microsoft.AspNetCore.Mvc;

namespace Web_Application.Controllers;

[ApiController]
[Route("[controller]")]
public class LeagueController : ControllerBase
{

    [HttpGet(Name = "")]
    public void Get()
    {
    }
}