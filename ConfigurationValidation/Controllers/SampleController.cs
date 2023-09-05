using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    private readonly SampleService _sampleService;

    public SampleController(SampleService sampleService)
    {
        _sampleService = sampleService;
    }

    [HttpGet]
    public IActionResult GetOptions()
    {
        var options = _sampleService.GetOptions();
        return Ok(options);
    }
}