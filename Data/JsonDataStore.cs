using SiteConstructor.Models;
using System.Text.Json;

namespace SiteConstructor.Data;
public class JsonDataStore : IDataStore
{
    private readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
    
    private readonly string path = Path.Combine(AppContext.BaseDirectory, "data.json");

    public async Task<Content> ReadAsync()
    {
        string json = await File.ReadAllTextAsync(path);
        Content content = JsonSerializer.Deserialize<Content>(json, options)!;
        return content;

    }
    public async Task WriteAsync(Content content)
    {
        string json = JsonSerializer.Serialize(content, options);
        await File.WriteAllTextAsync(path, json);
    }
}