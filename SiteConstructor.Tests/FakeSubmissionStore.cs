using SiteConstructor.Data;
using SiteConstructor.Models;

namespace SiteConstructor.Tests;

public class FakeSubmissionStore : ISubmissionStore
{
    private readonly List<Submission> _items = new();
    private int _nextId = 1;
    public Task<Submission> AddAsync(Submission submission)
    {
        submission.Id = _nextId;
        _nextId++;
        _items.Add(submission);
        return Task.FromResult(submission);
    }

    public Task<List<Submission>> GetAllAsync()
    {
        return Task.FromResult(_items.OrderByDescending(s => s.CreatedAt).ToList());
    }
}