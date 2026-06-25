namespace SiteConstructor.Models;

public class Content
{
    public Hero Hero{get; set;} = new();
    public List<TeamMember> Team{get; set;} = new();
    public List<Vacancy> Vacancies{get; set;} = new();

}

public class Hero
{
    public string Title{get; set;} = "";
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

public class Vacancy{
    public int Id{get; set;}
    public string Title{get; set;} = "";
    public string Format{get; set;} = "";
    public string Url{get; set;} = "";
}