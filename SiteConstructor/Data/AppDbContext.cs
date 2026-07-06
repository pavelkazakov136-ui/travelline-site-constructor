using Microsoft.EntityFrameworkCore;
using SiteConstructor.Models;
using System.Text.Json;

namespace SiteConstructor.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<TeamMember> TeamMembers{get; set;}
    public DbSet<Vacancy> Vacancies{get; set;}
    public DbSet<Client> Clients{get; set;}
    public DbSet<GalleryItem> GalleryItems {get; set;}
    public DbSet<Bonus> Bonuses {get; set;}
    public DbSet<SiteSettings> SiteSettings {get; set;}
    public DbSet<Submission> Submissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TeamMember>().Property(m => m.Id).ValueGeneratedNever();
        modelBuilder.Entity<Vacancy>().Property(v => v.Id).ValueGeneratedNever();
        modelBuilder.Entity<Client>().Property(c => c.Id).ValueGeneratedNever();
        modelBuilder.Entity<GalleryItem>().Property(g => g.Id).ValueGeneratedNever();
        modelBuilder.Entity<Bonus>().Property(b => b.Id).ValueGeneratedNever();
        modelBuilder.Entity<Submission>().Property(s => s.Id).ValueGeneratedNever();
        modelBuilder.Entity<SiteSettings>()

        .Property(s => s.Hero)
        .HasColumnType("jsonb")
        .HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<Hero>(v, (JsonSerializerOptions?)null)!);
            
        modelBuilder.Entity<SiteSettings>()
        .Property(s => s.Form)
        .HasColumnType("jsonb")
        .HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<Form>(v, (JsonSerializerOptions?)null)!);
    }

}