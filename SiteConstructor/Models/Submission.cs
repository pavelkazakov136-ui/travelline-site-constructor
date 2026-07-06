namespace SiteConstructor.Models;

public class Submission
{
    public int Id{get; set;}
    public string Name{get; set;} = "";
    public string Phone{get; set;} = "";
    public string Email{get; set;} = "";
    public string Resume{get; set;} = "";
    public string Direction{get; set;} = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
