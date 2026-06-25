using SiteConstructor.Models;

namespace SiteConstructor.Data;

public interface IDataStore
{
    Task<Content> ReadAsync();
    Task WriteAsync(Content content);
}

