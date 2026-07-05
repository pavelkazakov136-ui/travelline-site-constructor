using Microsoft.AspNetCore.Mvc;
using SiteConstructor.Models;
using SiteConstructor.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]
[Route("api/[controller]")]

public class ContentController : ControllerBase
{
    private readonly ContentService _service;
    public ContentController(ContentService service)
    {
        _service = service;
    }
    [AllowAnonymous] 
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

    [HttpPost("clients")]
    public async Task<IActionResult> AddClient([FromBody] Client client)
        => Ok(await _service.AddClientAsync(client));

    [HttpPut("clients/{id}")]
    public async Task<IActionResult> UpdateClient(int id, [FromBody] Client client)
    {
        var updated = await _service.UpdateClientAsync(id, client);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("clients/{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        await _service.DeleteClientAsync(id);
        return NoContent();
    }

    [HttpPost("gallery")]
    public async Task<IActionResult> AddGalleryItem([FromBody] GalleryItem galleryItem)
    {
        var created = await _service.AddGalleryItemAsync(galleryItem);
        return Ok(created);
    }

    [HttpPut("gallery/{id}")]
    public async Task<IActionResult> UpdateGalleryItem(int id, [FromBody] GalleryItem galleryItem)
    {
        var updated = await _service.UpdateGalleryItemAsync(id, galleryItem);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("gallery/{id}")]
    public async Task<IActionResult> DeleteGalleryItem(int id)
    {
        await _service.DeleteGalleryItemAsync(id);
        return NoContent();
    }

        [HttpPost("bonuses")]
    public async Task<IActionResult> AddBonus([FromBody] Bonus bonus)
    {
        var created = await _service.AddBonusAsync(bonus);
        return Ok(created);
    }

    [HttpPut("bonuses/{id}")]
    public async Task<IActionResult> UpdateBonus(int id, [FromBody] Bonus bonus)
    {
        var updated = await _service.UpdateBonusAsync(id, bonus);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("bonuses/{id}")]
    public async Task<IActionResult> DeleteBonus(int id)
    {
        await _service.DeleteBonusAsync(id);
        return NoContent();
    }
    
    [HttpPut("form")]
    public async Task<IActionResult> UpdateForm([FromBody] Form form)
    {
        var updated = await _service.UpdateFormAsync(form);
        return Ok(updated);
    }
} 