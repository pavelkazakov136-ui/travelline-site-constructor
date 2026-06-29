using SiteConstructor.Models;
using SiteConstructor.Services;
using Xunit;

namespace SiteConstructor.Tests;

public class ContentServiceTests
{
    [Fact]
    public async Task AddTeamMember_WhenOneMemberExists_AssignsIdTwo()
    {
        // Arrange — готовим стартовое состояние: в команде УЖЕ есть один (id=1)
        var content = new Content
        {
            Team = new List<TeamMember>
            {
                new TeamMember { Id = 1, Name = "Существующий", Position = "тест", Photo = "" }
            }
        };
        var service = new ContentService(new FakeDataStore(content));

        // Act — добавляем нового сотрудника
        var added = await service.AddTeamMemberAsync(
            new TeamMember { Name = "Новый", Position = "тест", Photo = "" });

        // Assert — новый должен получить id=2, а НЕ 1
        Assert.Equal(2, added.Id);
    }

    [Fact]
    public async Task AddTwo_ToAnEmptyCommand_IdsAreDifferent()
    {
        var content = new Content
        {
            Team = new List<TeamMember>()
        };
        var service = new ContentService(new FakeDataStore(content));

        var added1 = await service.AddTeamMemberAsync(
            new TeamMember { Name = "Новый1", Position = "тест", Photo = "" });
        var added2 = await service.AddTeamMemberAsync(
            new TeamMember { Name = "Новый2", Position = "тест", Photo = "" });

        Assert.Equal(1, added1.Id);
        Assert.Equal(2, added2.Id);
    }

    [Fact]
    public async Task UpdateANonExistentMember_OutputNull()
    {
        var content = new Content
        {
            Team = new List<TeamMember>()
        };
        var service = new ContentService(new FakeDataStore(content));

        var updated = await service.UpdateTeamMemberAsync(
            999,
            new TeamMember { Name = "изменить", Position = "тест", Photo = "" });

        Assert.Null(updated);
    }  

    [Fact]
    public async Task DeleteMember_ImTeamOfTwo_WillOnlyBeOneNeeded()
    {
        var content = new Content
        {
            Team = new List<TeamMember>
            {
                new TeamMember { Id = 1, Name = "Существующий1", Position = "тест", Photo = "" },
                new TeamMember { Id = 2, Name = "Существующий2", Position = "тест", Photo = "" }
            }
        };
        var service = new ContentService(new FakeDataStore(content));

        await service.DeleteTeamMemberAsync(1);

        Assert.Single(content.Team);
        Assert.Equal(2, content.Team[0].Id);
    } 

    [Fact]
    public async Task AddVacancy_WhenOneExists_AssignsIdTwo()
    {
        var content = new Content
        {
            Vacancies = new List<Vacancy>
            {
                new Vacancy { Id = 1, Title = "Существующая", Format = "офис", Url = "" }
            }
        };
        var service = new ContentService(new FakeDataStore(content));

        var added = await service.AddVacancyAsync(
            new Vacancy { Title = "Новая", Format = "удалённо", Url = "" });

        Assert.Equal(2, added.Id);
    }

    [Fact]
    public async Task UpdateVacancy_WhenNotExists_ReturnsNull()
    {
        var content = new Content { Vacancies = new List<Vacancy>() };
        var service = new ContentService(new FakeDataStore(content));

        var updated = await service.UpdateVacancyAsync(
            999, new Vacancy { Title = "x", Format = "x", Url = "" });

        Assert.Null(updated);
    }

    [Fact]
    public async Task DeleteVacancy_InListOfTwo_LeavesOnlyTheOther()
    {
        var content = new Content
        {
            Vacancies = new List<Vacancy>
            {
                new Vacancy { Id = 1, Title = "Первая", Format = "офис", Url = "" },
                new Vacancy { Id = 2, Title = "Вторая", Format = "удалённо", Url = "" }
            }
        };
        var service = new ContentService(new FakeDataStore(content));

        await service.DeleteVacancyAsync(1);

        Assert.Single(content.Vacancies);
        Assert.Equal(2, content.Vacancies[0].Id);
    }

    [Fact]
    public async Task UpdateHero_ReplacesHeroData()
    {
        // Arrange — стартовый hero со старым текстом
        var content = new Content
        {
            Hero = new Hero { Subtitle = "Старый текст", Stats = new List<Stat>() }
        };
        var service = new ContentService(new FakeDataStore(content));

        // Act — обновляем на новый hero
        var newHero = new Hero
        {
            Subtitle = "Новый текст",
            Stats = new List<Stat> { new Stat { Value = "300+", Label = "сотрудников" } }
        };
        await service.UpdateHeroAsync(newHero);

        // Assert — проверяем, что данные заменились
        Assert.Equal("Новый текст", content.Hero.Subtitle);
        Assert.Single(content.Hero.Stats);
        Assert.Equal("300+", content.Hero.Stats[0].Value);
    }
}