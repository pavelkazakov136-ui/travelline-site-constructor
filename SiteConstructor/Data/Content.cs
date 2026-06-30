namespace SiteConstructor.Models;

public class Content
{
    public Hero Hero{get; set;} = new();
    public List<TeamMember> Team{get; set;} = new();
    public List<Vacancy> Vacancies{get; set;} = new();
    public List<Client> Clients{get; set;} = new();
    public List<GalleryItem> Gallery{get; set;} = new();
    public List<Bonus> Bonuses{get; set;} = new();
    public Form Form{get; set;} = new();

}

public class Hero
{
    public string Subtitle{get; set;} = "";
    public List<Stat> Stats{get; set;} = new();
}

public class Stat
{
    public string Value{get; set;} = "";
    public string Label{get; set;} = "";
}

public class TeamMember
{
    public int Id{get; set;}
    public string Name{get; set;} = "";
    public string Position{get; set;} = "";
    public string Photo{get; set;} = "";
}

public class Vacancy
{
    public int Id{get; set;}
    public string Title{get; set;} = "";
    public string Format{get; set;} = "";
    public string Url{get; set;} = "";
}

public class Client
{
    public int Id{get; set;}
    public string Logo{get; set;} = "";
    public string Name{get; set;} = "";
}

public class Form
{
    public string Title{get; set;} = "";
    public string Subtitle{get; set;} = "";
    public string Button{get; set;} = "";
}

public class GalleryItem
{
    public int Id{get; set;}
    public string Title{get; set;} = "";
    public string Media{get; set;} = "";

}

public class Bonus
{
    public int Id{get; set;}
    public string Title{get; set;} = "";
    public string Subtitle{get; set;} = "";
}