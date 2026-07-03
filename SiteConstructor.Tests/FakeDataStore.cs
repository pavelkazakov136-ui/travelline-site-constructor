using SiteConstructor.Data;
using SiteConstructor.Models;
using System.Text.Json;

namespace SiteConstructor.Tests;

public class FakeDataStore : IDataStore
{
    private Content _content;  

    public FakeDataStore(Content content)
    {
        _content = content;     // тест задаёт начальное состояние
    }

    public Task<Content> ReadAsync()
    {
        var json = JsonSerializer.Serialize(_content);
        var copy = JsonSerializer.Deserialize<Content>(json)!;
        return Task.FromResult(copy);
    }

    public Task WriteAsync(Content content)
    {
        _content = content;                 // просто сохранить в памяти
        return Task.CompletedTask;
    }
}