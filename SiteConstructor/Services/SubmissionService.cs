namespace SiteConstructor.Services;

using SiteConstructor.Data;
using SiteConstructor.Models;

public class SubmissionService
{
    private readonly ISubmissionStore _store;
    public SubmissionService(ISubmissionStore store)
    {
        _store = store;
    }

    private void ValidateRequired(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{fieldName} is required and cannot be empty.");
        } 
    }
    
    public async Task<Submission> AddAsync(Submission submission)
    {
        ValidateRequired(submission.Name,"Name");
        ValidateRequired(submission.Phone,"Phone");
        ValidateRequired(submission.Email,"Email");
        ValidateRequired(submission.Resume,"Resume");
        submission.CreatedAt = DateTime.UtcNow;
        return await _store.AddAsync(submission);
    }
    
    public async Task<List<Submission>> GetAllAsync()
    {
        return await _store.GetAllAsync();
    }


}