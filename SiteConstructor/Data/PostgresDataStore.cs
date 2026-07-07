using SiteConstructor.Models;
using Microsoft.EntityFrameworkCore;
namespace SiteConstructor.Data;

public class PostgresDataStore : IDataStore{

    private readonly AppDbContext _db;
    public PostgresDataStore(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Content> ReadAsync()
        {
            var settings = await _db.SiteSettings.FirstOrDefaultAsync(s => s.Id == 1);

            return new Content
            {
                Hero = settings?.Hero ?? new Hero(),
                Form = settings?.Form ?? new Form(),
                Team = await _db.TeamMembers.OrderBy(m => m.Order).ToListAsync(),
                Vacancies = await _db.Vacancies.OrderBy(v => v.Order).ToListAsync(),
                Clients = await _db.Clients.OrderBy(c => c.Order).ToListAsync(),
                Gallery = await _db.GalleryItems.OrderBy(g => g.Order).ToListAsync(),
                Bonuses = await _db.Bonuses.OrderBy(b => b.Order).ToListAsync(),
            };
        }

    public async Task WriteAsync(Content content)
        {
            await using var tx = await _db.Database.BeginTransactionAsync();

            await _db.TeamMembers.ExecuteDeleteAsync();
            await _db.Vacancies.ExecuteDeleteAsync();
            await _db.Clients.ExecuteDeleteAsync();
            await _db.GalleryItems.ExecuteDeleteAsync();
            await _db.Bonuses.ExecuteDeleteAsync();

            _db.TeamMembers.AddRange(content.Team);
            _db.Vacancies.AddRange(content.Vacancies);
            _db.Clients.AddRange(content.Clients);
            _db.GalleryItems.AddRange(content.Gallery);
            _db.Bonuses.AddRange(content.Bonuses);
            var settings = await _db.SiteSettings.FirstOrDefaultAsync(s => s.Id == 1);
            if(settings == null)
            {
                settings = new SiteSettings
                {
                    Id = 1,
                    Hero = content.Hero,
                    Form = content.Form 
                };
                _db.SiteSettings.Add(settings);
            }
            else
            {
                settings.Hero = content.Hero;
                settings.Form = content.Form;
                _db.SiteSettings.Update(settings);
            }
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }
}