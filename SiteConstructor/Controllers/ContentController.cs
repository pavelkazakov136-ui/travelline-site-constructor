using Microsoft.AspNetCore.Mvc;
using SiteConstructor.Models;
using SiteConstructor.Services;

[ApiController]
[Route("api/[controller]")]

public class ContentController : ControllerBase
{
    private readonly ContentService _service;
    public ContentController(ContentService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var content = await _service.GetAsync();
        return Ok(content);
    }

    [HttpPut("hero")]
    public async Task<IActionResult> UpdateHero([FromBody] Hero hero)
    {
        var content = await _service.UpdateHeroAsync(hero);
        return Ok(content);
    }

   [HttpPost("team")]
    public async Task<IActionResult> AddTeamMember([FromBody] TeamMember member)
    {
        var created = await _service.AddTeamMemberAsync(member);
        return Ok(created);
    }
    [HttpPut("team/{id}")]
    public async Task<IActionResult> UpdateTeamMember(int id, [FromBody] TeamMember member)
    {
        var updated = await _service.UpdateTeamMemberAsync(id, member);
        return updated is null ? NotFound() : Ok(updated);   // вот где null → NotFound
    }

    [HttpDelete("team/{id}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        await _service.DeleteTeamMemberAsync(id);
        return NoContent();
    }
    
    [HttpPost("vacancies")]
    public async Task<IActionResult> AddVacancy([FromBody] Vacancy vacancy)
    {
        await _service.AddVacancyAsync(vacancy);
        return Ok(vacancy);
    }
    [HttpDelete("vacancies/{id}")]
    public async Task<IActionResult> DeleteVacancy(int id)
    {
        await _service.DeleteVacancyAsync(id);
        return NoContent();
    }
    [HttpPut("vacancies/{id}")]
    public async Task<IActionResult> UpdateVacancy(int id, [FromBody] Vacancy vacancy)
    {
        var updated = await _service.UpdateVacancyAsync(id, vacancy);
        return updated is null ? NotFound() : Ok(updated);

    }
} 