using Microsoft.CodeAnalysis.Completion;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models;

public class AppDbContext:DbContext
{
    public DbSet<ContactEntity> Contacts { get; set; }

    private string DbPath { get; set; } 
    
    public AppDbContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "Contacts.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data source = {DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactEntity>()
            .HasData(
                new ContactEntity()
                {
                    Id = 1, First_name = "krull", Last_name = "legowski", Email = "krol@legowski.pl",
                    Phonenumber = "512323123", Date_of_Birth = new DateOnly(2000, 10, 10)
                },
                new ContactEntity()
                {
                    Id = 2, First_name = "masia", Last_name = "miszcz", Email = "masia@leggsdagowski.pl",
                    Phonenumber = "999666333", Date_of_Birth = new DateOnly(2000, 10, 10)
                }
            );
    }
}