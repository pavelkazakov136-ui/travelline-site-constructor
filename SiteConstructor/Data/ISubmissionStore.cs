using SiteConstructor.Models;

namespace SiteConstructor.Data;

public interface ISubmissionStore
{
    Task<Submission> AddAsync(Submission submission);
    Task<List<Submission>> GetAllAsync();   
    Task DeleteAsync(int id); 
}