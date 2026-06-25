using Microsoft.AspNetCore.Mvc;
using SiteConstructor.Data;

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



}