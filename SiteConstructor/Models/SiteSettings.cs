namespace SiteConstructor.Models;

public class SiteSettings
{
    public int Id{get; set;}
    public Hero Hero{get; set;} = new();
    public Form Form{get; set;} = new();
}