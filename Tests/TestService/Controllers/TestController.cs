using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestService.Models;

namespace TestService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDefaultString()
    {
        return Ok("Hello world");
    }

    [HttpGet]
    public async Task<IActionResult> GetDefaultObject()
    {
        return Ok(new Person { Id = -1, Name = "TestName" });
    }
}
