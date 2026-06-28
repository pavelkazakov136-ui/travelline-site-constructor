using Microsoft.AspNetCore.Mvc;
using SiteConstructor.Data;
using SiteConstructor.Models;

[ApiController]
[Route("api/[controller]")]

public class ContentController : ControllerBase
{
    private readonly IDataStore _store;
    public ContentController(IDataStore store)
    {
        _store = store;
    }
    [HttpGet] // GET /api/content
    public async Task<IActionResult> Get()
    {
        var content = await _store.ReadAsync();
        return Ok(content);
    }

    [HttpPut("hero")]
    public async Task<IActionResult> UpdateHero([FromBody] Hero hero)
    {
        var content = await _store.ReadAsync();
        content.Hero = hero;
        await _store.WriteAsync(content);
        return Ok(content);
    }

    [HttpPut("team/{id}")]
    public async Task<IActionResult> UpdateTeamMember(int id, [FromBody] TeamMember member)
    {
        var content = await _store.ReadAsync();
        var existing = content.Team.FirstOrDefault(m => m.Id == id);
        if (existing is null) return NotFound();
        existing.Name = member.Name;
        existing.Position = member.Position;
        existing.Photo = member.Photo;
        await _store.WriteAsync(content);
        return Ok(existing);
    }

    [HttpPost("team")]
    public async Task<IActionResult> AddTeamMember([FromBody] TeamMember member)
    {
        var content = await _store.ReadAsync();
        member.Id = content.Team.Count > 0 ? content.Team.Max(m => m.Id) + 1 : 1;
        content.Team.Add(member);
        await _store.WriteAsync(content);
        return Ok(member);
    }

    [HttpDelete("team/{id}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        var content = await _store.ReadAsync();
        content.Team.RemoveAll(m => m.Id == id);
        await _store.WriteAsync(content);
        return NoContent();
    }
    [HttpPost("vacancies")]
    public async Task<IActionResult> AddVacancy([FromBody] Vacancy vacancy)
    {
        var content = await _store.ReadAsync();
        vacancy.Id = content.Vacancies.Count > 0 ? content.Vacancies.Max(v => v.Id) + 1 : 1;
        content.Vacancies.Add(vacancy);
        await _store.WriteAsync(content);
        return Ok(vacancy);
    }
    [HttpDelete("vacancies/{id}")]
    public async Task<IActionResult> DeleteVacancy(int id)
    {
        var content = await _store.ReadAsync();
        content.Vacancies.RemoveAll(v => v.Id == id);
        await _store.WriteAsync(content);
        return NoContent();
    }
    [HttpPut("vacancies/{id}")]
    public async Task<IActionResult> UpdateVacancy(int id, [FromBody] Vacancy vacancy)
    {
        var content = await _store.ReadAsync();
        var existing = content.Vacancies.FirstOrDefault(v => v.Id == id);
        if (existing is null) return NotFound();
        existing.Title =  vacancy.Title;
        existing.Format = vacancy.Format;
        existing.Url = vacancy.Url;
        await _store.WriteAsync(content);
        return Ok(existing);

    }
} 