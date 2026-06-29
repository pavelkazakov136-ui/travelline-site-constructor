using SiteConstructor.Models;
using SiteConstructor.Data;

namespace SiteConstructor.Services;

public class ContentService
{
    private readonly IDataStore _store;
    public ContentService(IDataStore store)
    {
        _store = store;
    }

    public async Task<Content> GetAsync()
    {
        var content = await _store.ReadAsync();
        return content;
    }

    public async Task<Content> UpdateHeroAsync(Hero hero)
    {
        var content = await _store.ReadAsync();
        content.Hero = hero;
        await _store.WriteAsync(content);
        return content;
    }

    public async Task<TeamMember> AddTeamMemberAsync(TeamMember member)
    {
        var content = await _store.ReadAsync();
        member.Id = content.Team.Count > 0 ? content.Team.Max(m => m.Id) + 1 : 1;
        content.Team.Add(member);
        await _store.WriteAsync(content);
        return member;
    }
    public async Task<TeamMember?> UpdateTeamMemberAsync(int id, TeamMember member)
    {
        var content = await _store.ReadAsync();
        var existing = content.Team.FirstOrDefault(m => m.Id == id);
        if (existing is null) return null;
        existing.Name = member.Name;
        existing.Position = member.Position;
        existing.Photo = member.Photo;
        await _store.WriteAsync(content);
        return existing;
    }

    public async Task DeleteTeamMemberAsync(int id)
    {
        var content = await _store.ReadAsync();
        content.Team.RemoveAll(m => m.Id == id);
        await _store.WriteAsync(content);
    }

    public async Task<Vacancy> AddVacancyAsync(Vacancy vacancy)
    {
        var content = await _store.ReadAsync();
        vacancy.Id = content.Vacancies.Count > 0 ? content.Vacancies.Max(v => v.Id) + 1 : 1;
        content.Vacancies.Add(vacancy);
        await _store.WriteAsync(content);
        return vacancy;
    }
    public async Task DeleteVacancyAsync(int id)
    {
        var content = await _store.ReadAsync();
        content.Vacancies.RemoveAll(v => v.Id == id);
        await _store.WriteAsync(content);
    }
    public async Task<Vacancy?> UpdateVacancyAsync(int id, Vacancy vacancy)
    {
        var content = await _store.ReadAsync();
        var existing = content.Vacancies.FirstOrDefault(v => v.Id == id);
        if (existing is null) return null;
        existing.Title =  vacancy.Title;
        existing.Format = vacancy.Format;
        existing.Url = vacancy.Url;
        await _store.WriteAsync(content);
        return existing;

    }
}