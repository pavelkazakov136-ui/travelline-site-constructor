using SiteConstructor.Models;
using SiteConstructor.Services;
using Xunit;

namespace SiteConstructor.Tests;

public class ContentServiceTests
{
    [Fact]
    public async Task AddTeamMember_WhenOneMemberExists_AssignsIdTwo_AndPersists()
    {
        var store = new FakeDataStore(new Content
        {
            Team = new List<TeamMember>
            {
                new TeamMember { Id = 1, Name = "Существующий", Position = "тест", Photo = "" }
            }
        });
        var service = new ContentService(store);

        var added = await service.AddTeamMemberAsync(
            new TeamMember { Name = "Новый", Position = "тест", Photo = "" });

        Assert.Equal(2, added.Id);

        var after = await store.ReadAsync();
        Assert.Equal(2, after.Team.Count);
        Assert.Contains(after.Team, m => m.Id == 2 && m.Name == "Новый");
    }

    [Fact]
    public async Task AddTwoMembers_ToAnEmptyTeam_IdsAreDifferent()
    {
        var store = new FakeDataStore(new Content { Team = new List<TeamMember>() });
        var service = new ContentService(store);

        var added1 = await service.AddTeamMemberAsync(
            new TeamMember { Name = "Новый1", Position = "тест", Photo = "" });
        var added2 = await service.AddTeamMemberAsync(
            new TeamMember { Name = "Новый2", Position = "тест", Photo = "" });

        Assert.Equal(1, added1.Id);
        Assert.Equal(2, added2.Id);
    }

    [Fact]
    public async Task UpdateTeamMember_WhenNotExists_ReturnsNull()
    {
        var store = new FakeDataStore(new Content { Team = new List<TeamMember>() });
        var service = new ContentService(store);

        var updated = await service.UpdateTeamMemberAsync(
            999,
            new TeamMember { Name = "изменить", Position = "тест", Photo = "" });

        Assert.Null(updated);
    }

    [Fact]
    public async Task UpdateTeamMember_WhenExists_ChangesArePersisted()
    {
        var store = new FakeDataStore(new Content
        {
            Team = new List<TeamMember>
            {
                new TeamMember { Id = 1, Name = "Старое имя", Position = "junior", Photo = "" }
            }
        });
        var service = new ContentService(store);

        await service.UpdateTeamMemberAsync(1,
            new TeamMember { Name = "Новое имя", Position = "senior", Photo = "/img/x.png" });

        var after = await store.ReadAsync();
        var member = Assert.Single(after.Team);
        Assert.Equal("Новое имя", member.Name);
        Assert.Equal("senior", member.Position);
    }

    [Fact]
    public async Task DeleteTeamMember_InTeamOfTwo_OnlyTheOtherRemains()
    {
        var store = new FakeDataStore(new Content
        {
            Team = new List<TeamMember>
            {
                new TeamMember { Id = 1, Name = "Существующий1", Position = "тест", Photo = "" },
                new TeamMember { Id = 2, Name = "Существующий2", Position = "тест", Photo = "" }
            }
        });
        var service = new ContentService(store);

        await service.DeleteTeamMemberAsync(1);

        var after = await store.ReadAsync();
        Assert.Single(after.Team);
        Assert.Equal(2, after.Team[0].Id);
    }


    [Fact]
    public async Task AddVacancy_WhenOneExists_AssignsIdTwo_AndPersists()
    {
        var store = new FakeDataStore(new Content
        {
            Vacancies = new List<Vacancy>
            {
                new Vacancy { Id = 1, Title = "Существующая", Format = "офис", Url = "" }
            }
        });
        var service = new ContentService(store);

        var added = await service.AddVacancyAsync(
            new Vacancy { Title = "Новая", Format = "удалённо", Url = "" });

        Assert.Equal(2, added.Id);

        var after = await store.ReadAsync();
        Assert.Equal(2, after.Vacancies.Count);
    }

    [Fact]
    public async Task UpdateVacancy_WhenNotExists_ReturnsNull()
    {
        var store = new FakeDataStore(new Content { Vacancies = new List<Vacancy>() });
        var service = new ContentService(store);

        var updated = await service.UpdateVacancyAsync(
            999, new Vacancy { Title = "x", Format = "x", Url = "" });

        Assert.Null(updated);
    }

    [Fact]
    public async Task DeleteVacancy_InListOfTwo_LeavesOnlyTheOther()
    {
        var store = new FakeDataStore(new Content
        {
            Vacancies = new List<Vacancy>
            {
                new Vacancy { Id = 1, Title = "Первая", Format = "офис", Url = "" },
                new Vacancy { Id = 2, Title = "Вторая", Format = "удалённо", Url = "" }
            }
        });
        var service = new ContentService(store);

        await service.DeleteVacancyAsync(1);

        var after = await store.ReadAsync();
        Assert.Single(after.Vacancies);
        Assert.Equal(2, after.Vacancies[0].Id);
    }

    [Fact]
    public async Task UpdateHero_ReplacesHeroData_AndPersists()
    {
        var store = new FakeDataStore(new Content
        {
            Hero = new Hero { Subtitle = "Старый текст", Stats = new List<Stat>() }
        });
        var service = new ContentService(store);

        await service.UpdateHeroAsync(new Hero
        {
            Subtitle = "Новый текст",
            Stats = new List<Stat> { new Stat { Value = "300+", Label = "сотрудников" } }
        });

        var after = await store.ReadAsync();
        Assert.Equal("Новый текст", after.Hero.Subtitle);
        var stat = Assert.Single(after.Hero.Stats);
        Assert.Equal("300+", stat.Value);
    }

    [Fact]
    public async Task AddClient_WhenOneExists_AssignsIdTwo()
    {
        var store = new FakeDataStore(new Content
        {
            Clients = new List<Client>
            {
                new Client { Id = 1, Logo = "x.svg", Name = "Existing" }
            }
        });
        var service = new ContentService(store);

        var added = await service.AddClientAsync(new Client { Logo = "new.svg", Name = "New" });

        Assert.Equal(2, added.Id);

        var after = await store.ReadAsync();
        Assert.Equal(2, after.Clients.Count);
    }

    [Fact]
    public async Task UpdateClient_WhenNotExists_ReturnsNull()
    {
        var store = new FakeDataStore(new Content { Clients = new List<Client>() });
        var service = new ContentService(store);

        var updated = await service.UpdateClientAsync(999, new Client { Logo = "x", Name = "y" });

        Assert.Null(updated);
    }

    [Fact]
    public async Task DeleteClient_InListOfTwo_LeavesOnlyTheOther()
    {
        var store = new FakeDataStore(new Content
        {
            Clients = new List<Client>
            {
                new Client { Id = 1, Logo = "a", Name = "A" },
                new Client { Id = 2, Logo = "b", Name = "B" }
            }
        });
        var service = new ContentService(store);

        await service.DeleteClientAsync(1);

        var after = await store.ReadAsync();
        Assert.Single(after.Clients);
        Assert.Equal(2, after.Clients[0].Id);
    }

    [Fact]
    public async Task AddGalleryItem_WhenOneExists_AssignsIdTwo()
    {
        var store = new FakeDataStore(new Content
        {
            Gallery = new List<GalleryItem>
            {
                new GalleryItem { Id = 1, Media = "x.svg", Title = "Existing" }
            }
        });
        var service = new ContentService(store);

        var added = await service.AddGalleryItemAsync(
            new GalleryItem { Media = "new.svg", Title = "New" });

        Assert.Equal(2, added.Id);

        var after = await store.ReadAsync();
        Assert.Equal(2, after.Gallery.Count);
    }

    [Fact]
    public async Task UpdateGalleryItem_WhenNotExists_ReturnsNull()
    {
        var store = new FakeDataStore(new Content { Gallery = new List<GalleryItem>() });
        var service = new ContentService(store);

        var updated = await service.UpdateGalleryItemAsync(
            999, new GalleryItem { Title = "x", Media = "y" });

        Assert.Null(updated);
    }

    [Fact]
    public async Task DeleteGalleryItem_InListOfTwo_LeavesOnlyTheOther()
    {
        var store = new FakeDataStore(new Content
        {
            Gallery = new List<GalleryItem>
            {
                new GalleryItem { Id = 1, Title = "a", Media = "A" },
                new GalleryItem { Id = 2, Title = "b", Media = "B" }
            }
        });
        var service = new ContentService(store);

        await service.DeleteGalleryItemAsync(1);

        var after = await store.ReadAsync();
        Assert.Single(after.Gallery);
        Assert.Equal(2, after.Gallery[0].Id);
    }


    [Fact]
    public async Task AddBonus_WhenOneExists_AssignsIdTwo()
    {
        var store = new FakeDataStore(new Content
        {
            Bonuses = new List<Bonus>
            {
                new Bonus { Id = 1, Title = "Существующий", Subtitle = "Existing" }
            }
        });
        var service = new ContentService(store);

        var added = await service.AddBonusAsync(new Bonus { Title = "Новый", Subtitle = "New" });

        Assert.Equal(2, added.Id);

        var after = await store.ReadAsync();
        Assert.Equal(2, after.Bonuses.Count);
    }

    [Fact]
    public async Task UpdateBonus_WhenNotExists_ReturnsNull()
    {
        var store = new FakeDataStore(new Content { Bonuses = new List<Bonus>() });
        var service = new ContentService(store);

        var updated = await service.UpdateBonusAsync(999, new Bonus { Title = "x", Subtitle = "y" });

        Assert.Null(updated);
    }

    [Fact]
    public async Task DeleteBonus_InListOfTwo_LeavesOnlyTheOther()
    {
        var store = new FakeDataStore(new Content
        {
            Bonuses = new List<Bonus>
            {
                new Bonus { Id = 1, Title = "a", Subtitle = "A" },
                new Bonus { Id = 2, Title = "b", Subtitle = "B" }
            }
        });
        var service = new ContentService(store);

        await service.DeleteBonusAsync(1);

        var after = await store.ReadAsync();
        Assert.Single(after.Bonuses);
        Assert.Equal(2, after.Bonuses[0].Id);
    }


    [Fact]
    public async Task UpdateForm_ReplacesFormData_AndPersists()
    {
        var store = new FakeDataStore(new Content
        {
            Form = new Form { Title = "old", Subtitle = "old", Button = "old" }
        });
        var service = new ContentService(store);

        await service.UpdateFormAsync(new Form { Title = "new", Subtitle = "new", Button = "new" });

        var after = await store.ReadAsync();
        Assert.Equal("new", after.Form.Title);
        Assert.Equal("new", after.Form.Button);
    }


    [Fact]
    public async Task AddTeamMember_WithEmptyName_ThrowsArgumentException()
    {
        var store = new FakeDataStore(new Content { Team = new List<TeamMember>() });
        var service = new ContentService(store);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AddTeamMemberAsync(new TeamMember { Name = "", Position = "тест", Photo = "" }));
    }

    [Fact]
    public async Task AddTeamMember_WithWhitespaceName_ThrowsArgumentException()
    {
        var store = new FakeDataStore(new Content { Team = new List<TeamMember>() });
        var service = new ContentService(store);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AddTeamMemberAsync(new TeamMember { Name = "   ", Position = "тест", Photo = "" }));
    }

    [Fact]
    public async Task AddTeamMember_WithEmptyName_DoesNotModifyStore()
    {
        var store = new FakeDataStore(new Content { Team = new List<TeamMember>() });
        var service = new ContentService(store);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AddTeamMemberAsync(new TeamMember { Name = "", Position = "", Photo = "" }));

        var after = await store.ReadAsync();
        Assert.Empty(after.Team);
    }

    [Fact]
    public async Task UpdateTeamMember_WithEmptyName_Throws_AndStoreIsUnchanged()
    {
        var store = new FakeDataStore(new Content
        {
            Team = new List<TeamMember>
            {
                new TeamMember { Id = 1, Name = "Исходный", Position = "тест", Photo = "" }
            }
        });
        var service = new ContentService(store);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UpdateTeamMemberAsync(1, new TeamMember { Name = "", Position = "x", Photo = "" }));

        var after = await store.ReadAsync();
        Assert.Equal("Исходный", after.Team[0].Name);
    }

    [Fact]
    public async Task AddTeamMember_ValidAfterInvalid_WorksAndAssignsCorrectId()
    {
        var store = new FakeDataStore(new Content { Team = new List<TeamMember>() });
        var service = new ContentService(store);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AddTeamMemberAsync(new TeamMember { Name = "", Position = "", Photo = "" }));

        var added = await service.AddTeamMemberAsync(
            new TeamMember { Name = "Валидный", Position = "тест", Photo = "" });

        Assert.Equal(1, added.Id); 
    }

    [Fact]
    public async Task AddVacancy_WithEmptyTitle_ThrowsArgumentException()
    {
        var store = new FakeDataStore(new Content { Vacancies = new List<Vacancy>() });
        var service = new ContentService(store);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AddVacancyAsync(new Vacancy { Title = "", Format = "офис", Url = "" }));
    }

    [Fact]
    public async Task AddClient_WithEmptyName_ThrowsArgumentException()
    {
        var store = new FakeDataStore(new Content { Clients = new List<Client>() });
        var service = new ContentService(store);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AddClientAsync(new Client { Name = "", Logo = "x.svg" }));
    }

    [Fact]
    public async Task AddGalleryItem_WithEmptyTitle_ThrowsArgumentException()
    {
        var store = new FakeDataStore(new Content { Gallery = new List<GalleryItem>() });
        var service = new ContentService(store);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AddGalleryItemAsync(new GalleryItem { Title = "", Media = "x.mp4" }));
    }

    [Fact]
    public async Task AddBonus_WithEmptyTitle_ThrowsArgumentException()
    {
        var store = new FakeDataStore(new Content { Bonuses = new List<Bonus>() });
        var service = new ContentService(store);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AddBonusAsync(new Bonus { Title = "", Subtitle = "x" }));
    }
}