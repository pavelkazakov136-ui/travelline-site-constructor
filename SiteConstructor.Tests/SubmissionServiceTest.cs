using SiteConstructor.Models;
using SiteConstructor.Services;
using Xunit;

namespace SiteConstructor.Tests;

public class SubmissionServiceTests
{
    [Fact]
    public async Task AddAsync_ValidSubmission_SavesAndAssignsId()
    {
        var store = new FakeSubmissionStore();
        var service = new SubmissionService(store);

        var result = await service.AddAsync(new Submission
        {
            Name = "Иван",
            Phone = "+79001234567",
            Email = "ivan@mail.ru",
            Resume = "https://hh.ru/resume/123"
        });

        Assert.Equal(1, result.Id);
        var all = await store.GetAllAsync();
        Assert.Single(all);       
    }

[Fact]
    public async Task AddAsync_EmptyName_ThrowsArgumentException()
    {
        var store = new FakeSubmissionStore();
        var service = new SubmissionService(store);

        // валидная во всём, КРОМЕ имени
        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AddAsync(new Submission
            {
                Name = "",
                Phone = "+7900",
                Email = "a@b.ru",
                Resume = "https://hh.ru/1"
            }));
        var all = await store.GetAllAsync();
        Assert.Empty(all);
    }

    [Fact]
    public async Task AddAsync_SetsCreatedAtToUtcNow()
    {
        var store = new FakeSubmissionStore();
        var service = new SubmissionService(store);

        var result = await service.AddAsync(new Submission
        {
            Name = "Иван",
            Phone = "+79001234567",
            Email = "ivan@mail.ru",
            Resume = "https://hh.ru/resume/123"
        });

        Assert.True(result.CreatedAt > DateTime.UtcNow.AddMinutes(-1));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsNewestFirst()
    {
        var store = new FakeSubmissionStore();

        var older = await store.AddAsync(new Submission { Name = "Старая" });
        var newer = await store.AddAsync(new Submission { Name = "Новая" });

        older.CreatedAt = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        newer.CreatedAt = new DateTime(2020, 1, 2, 0, 0, 0, DateTimeKind.Utc);

        var all = await store.GetAllAsync();

        Assert.Equal("Новая", all[0].Name);
        Assert.Equal("Старая", all[1].Name);
    }

}