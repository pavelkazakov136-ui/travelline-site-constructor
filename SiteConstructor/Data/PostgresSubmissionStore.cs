namespace SiteConstructor.Data; 
using SiteConstructor.Models;
using Microsoft.EntityFrameworkCore;

public class PostgresSubmissionStore : ISubmissionStore{
    private readonly AppDbContext _db;
        public PostgresSubmissionStore(AppDbContext db)
        {
            _db = db;
        }
    public async Task<Submission> AddAsync(Submission submission)
    {
        submission.Id = 0;
        _db.Submissions.Add(submission);
        await _db.SaveChangesAsync();
        return submission;
    }
    public async Task<List<Submission>> GetAllAsync()
    {
    return await _db.Submissions
        .OrderByDescending(s => s.CreatedAt)
        .ToListAsync();
    }
    public async Task DeleteAsync(int id)
{
    await _db.Submissions
        .Where(s => s.Id == id)
        .ExecuteDeleteAsync();
}
}