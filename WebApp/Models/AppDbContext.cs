using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace WebApp.Models;

public class AppDbContext : IdentityDbContext<IdentityUser>
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
        base.OnModelCreating(modelBuilder);
        var ADMIN_ID = Guid.NewGuid().ToString();
        var ADMIN_ROLE_ID = Guid.NewGuid().ToString();
        var USER_ID = Guid.NewGuid().ToString();
        var USER_ROLE_ID = Guid.NewGuid().ToString();
        modelBuilder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole()
                {
                    Id = ADMIN_ROLE_ID,
                    Name = "admin",
                    NormalizedName = "admin".ToUpper(),
                    ConcurrencyStamp = ADMIN_ROLE_ID
                },
                new IdentityRole()
                    {
                     Id   = USER_ROLE_ID,
                     Name = "user",
                     NormalizedName = "user".ToUpper(),
                     ConcurrencyStamp = USER_ROLE_ID
                    }
            );
        var admin = new IdentityUser()
        {
            Id = ADMIN_ID,
            UserName = "michal",
            NormalizedUserName = "michal".ToUpper(),
            Email = "michal@wsei.pl",
            NormalizedEmail = "michal@wsei.pl".ToUpper(),
            EmailConfirmed = true
        };
        PasswordHasher<IdentityUser> hasher = new PasswordHasher<IdentityUser>();

        var user = new IdentityUser()
        {
            Id = USER_ID,
            UserName = "luki",
            NormalizedUserName = "luki".ToUpper(),
            Email = "lek@wsei.pl",
            NormalizedEmail = "lek@wsei.pl".ToUpper(),
            EmailConfirmed = true
        };
        modelBuilder.Entity<IdentityUser>()
            .HasData(
                admin,
                user
            );
        admin.PasswordHash = hasher.HashPassword(admin, "1234");
        user.PasswordHash = hasher.HashPassword(user, "6666");
        modelBuilder.Entity<IdentityUserRole<string>>()
            .HasData(
                new IdentityUserRole<string>()
                {
                    RoleId = ADMIN_ROLE_ID,
                    UserId = ADMIN_ID
                },
                new IdentityUserRole<string>()
                {
                    RoleId = USER_ROLE_ID, 
                    UserId = USER_ID
                },
                    new IdentityUserRole<string>()
                    {
                        RoleId = USER_ROLE_ID,
                        UserId = ADMIN_ID
                    }
                
            );
        
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