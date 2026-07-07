using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiteConstructor.Models;
using SiteConstructor.Services;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class SubmissionsController : ControllerBase
{
    private readonly SubmissionService _service;
    public SubmissionsController(SubmissionService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Submission submission)
    {
        var created = await _service.AddAsync(submission);
        return Ok(created);
    }

    [HttpGet]                   
    public async Task<IActionResult> GetAll()
    {
        var all = await _service.GetAllAsync();
        return Ok(all);
    }
}