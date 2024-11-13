using Microsoft.CodeAnalysis.Completion;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models;

public class AppDbContext:DbContext
{
    public DbSet<ContactEntity> Contacts { get; set; }
    public DbSet<OrganizationEntity> Organizations { get; set; }

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
        modelBuilder.Entity<OrganizationEntity>()
            .ToTable("Organizations")
            .HasData(
                new OrganizationEntity()
                {
                    Id = 101, Name = "NazwaOrganizacji", NIP = "4567781234", REGON = "1234567890"
                },
                new OrganizationEntity()
                    {
                        Id = 102, Name = "NastepnaOrganizacja", NIP = "49157365", REGON = "5614693179"
                    }
                );
        
        modelBuilder.Entity<ContactEntity>()
            .Property(c => c.OrganizationId)
            .HasDefaultValue(100);
        
        modelBuilder.Entity<OrganizationEntity>()
            .OwnsOne(o => o.Address)
            .HasData(
                new {OrganizationEntityId = 101, Street = "Św. Filipa 17", City = "Kraków"},
                new {OrganizationEntityId = 102, Street = "Szpitalna 1", City = "Kraków"}
            );
        
        modelBuilder.Entity<ContactEntity>()
            .HasData(
                new ContactEntity()
                {
                    Id = 1, First_name = "krull", Last_name = "legowski", Email = "krol@legowski.pl",
                    Phonenumber = "512323123", Date_of_Birth = new DateOnly(2000, 10, 10),Created = DateTime.Now,OrganizationId = 101
                },
                new ContactEntity()
                {
                    Id = 2, First_name = "masia", Last_name = "miszcz", Email = "masia@leggsdagowski.pl",
                    Phonenumber = "999666333", Date_of_Birth = new DateOnly(2000, 10, 10),Created = DateTime.Now ,OrganizationId = 102
                }
            );
    }
}