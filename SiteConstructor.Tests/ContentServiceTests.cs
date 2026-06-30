using SiteConstructor.Models;
using SiteConstructor.Services;
using Xunit;

namespace SiteConstructor.Tests;

public class ContentServiceTests
{
    [Fact]
    public async Task AddTeamMember_WhenOneMemberExists_AssignsIdTwo()
    {
        var content = new Content
        {
            Team = new List<TeamMember>
            {
                new TeamMember { Id = 1, Name = "Существующий", Position = "тест", Photo = "" }
            }
        };
        var service = new ContentService(new FakeDataStore(content));

        var added = await service.AddTeamMemberAsync(
            new TeamMember { Name = "Новый", Position = "тест", Photo = "" });

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
        var content = new Content
        {
            Hero = new Hero { Subtitle = "Старый текст", Stats = new List<Stat>() }
        };
        var service = new ContentService(new FakeDataStore(content));

        var newHero = new Hero
        {
            Subtitle = "Новый текст",
            Stats = new List<Stat> { new Stat { Value = "300+", Label = "сотрудников" } }
        };
        await service.UpdateHeroAsync(newHero);

        Assert.Equal("Новый текст", content.Hero.Subtitle);
        Assert.Single(content.Hero.Stats);
        Assert.Equal("300+", content.Hero.Stats[0].Value);
    }

    [Fact]
public async Task AddClient_WhenOneExists_AssignsIdTwo()
    {
        var content = new Content
        {
            Clients = new List<Client>
            {
                new Client { Id = 1, Logo = "x.svg", Name = "Existing" }
            }
        };
        var service = new ContentService(new FakeDataStore(content));

        var added = await service.AddClientAsync(new Client { Logo = "new.svg", Name = "New" });

        Assert.Equal(2, added.Id);
    }

[Fact]
public async Task UpdateClient_WhenNotExists_ReturnsNull()
    {
        var content = new Content 
        { 
            Clients = new List<Client>()
        };
        var service = new ContentService(new FakeDataStore(content));

        var updated = await service.UpdateClientAsync(999, new Client { Logo = "x", Name = "y" });

        Assert.Null(updated);
    }

[Fact]
public async Task DeleteClient_InListOfTwo_LeavesOnlyTheOther()
    {
        var content = new Content
        {
            Clients = new List<Client>
            {
                new Client { Id = 1, Logo = "a", Name = "A" },
                new Client { Id = 2, Logo = "b", Name = "B" }
            }
        };
        var service = new ContentService(new FakeDataStore(content));

        await service.DeleteClientAsync(1);

        Assert.Single(content.Clients);
        Assert.Equal(2, content.Clients[0].Id);
    }

    [Fact]
public async Task AddGalleryItem_WhenOneExists_AssignsIdTwo()
    {
        var content = new Content
        {
            Gallery = new List<GalleryItem>
            {
                new GalleryItem { Id = 1, Media = "x.svg", Title = "Existing" }
            }
        };
        var service = new ContentService(new FakeDataStore(content));

        var added = await service.AddGalleryItemAsync(new GalleryItem { Media = "new.svg", Title = "New" });

        Assert.Equal(2, added.Id);
    }

[Fact]
    public async Task UpdateGalleryItem_WhenNotExists_ReturnsNull()
    {
        var content = new Content 
        { 
            Gallery = new List<GalleryItem>()
        };
        var service = new ContentService(new FakeDataStore(content));

        var updated = await service.UpdateGalleryItemAsync(999, new GalleryItem { Title = "x", Media = "y" });

        Assert.Null(updated);
    }

[Fact]
    public async Task DeleteGalleryItem_InListOfTwo_LeavesOnlyTheOther()
    {
        var content = new Content
        {
            Gallery = new List<GalleryItem>
            {
                new GalleryItem { Id = 1, Title = "a", Media = "A" },
                new GalleryItem { Id = 2, Title = "b", Media = "B" }
            }
        };
        var service = new ContentService(new FakeDataStore(content));

        await service.DeleteGalleryItemAsync(1);

        Assert.Single(content.Gallery);
        Assert.Equal(2, content.Gallery[0].Id);
    }

    [Fact]
    public async Task AddBonus_WhenOneExists_AssignsIdTwo()
    {
        var content = new Content
        {
            Bonuses = new List<Bonus>
            {
                new Bonus { Id = 1, Title = "x.svg", Subtitle = "Existing" }
            }
        };
        var service = new ContentService(new FakeDataStore(content));

        var added = await service.AddBonusAsync(new Bonus { Title = "new.svg", Subtitle = "New" });

        Assert.Equal(2, added.Id);
    }

[Fact]
    public async Task UpdateBonus_WhenNotExists_ReturnsNull()
    {
        var content = new Content 
        { 
            Bonuses = new List<Bonus>()
        };
        var service = new ContentService(new FakeDataStore(content));

        var updated = await service.UpdateBonusAsync(999, new Bonus { Title = "x", Subtitle = "y" });

        Assert.Null(updated);
    }

[Fact]
    public async Task DeleteBonus_InListOfTwo_LeavesOnlyTheOther()
    {
        var content = new Content
        {
            Bonuses = new List<Bonus>
            {
                new Bonus { Id = 1, Title = "a", Subtitle = "A" },
                new Bonus { Id = 2, Title = "b", Subtitle = "B" }
            }
        };
        var service = new ContentService(new FakeDataStore(content));

        await service.DeleteBonusAsync(1);

        Assert.Single(content.Bonuses);
        Assert.Equal(2, content.Bonuses[0].Id);
    }

[Fact]
    public async Task UpdateForm_ReplacesFormData()
    {
        var content = new Content
        {
            Form = {Title = "old", Subtitle = "old", Button = "old"}    
        };

        var service = new ContentService(new FakeDataStore(content));

        await service.UpdateFormAsync(new Form {Title = "new", Subtitle = "new", Button = "new"});

        Assert.Equal("new", content.Form.Title);
    }


}