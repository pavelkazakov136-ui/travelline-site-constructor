using SiteConstructor.Data;
using SiteConstructor.Models;

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
        return Task.FromResult(_content);   // просто вернуть из памяти
    }

    public Task WriteAsync(Content content)
    {
        _content = content;                 // просто сохранить в памяти
        return Task.CompletedTask;
    }
}