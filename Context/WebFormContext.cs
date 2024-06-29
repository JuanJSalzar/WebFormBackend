using Microsoft.EntityFrameworkCore;
using WebForm.Models;

namespace WebForm.Context;

public class WebFormContext : DbContext
{
    public DbSet<Person> People { get; set; }
    public DbSet<User> Users { get; set; }

    public WebFormContext(DbContextOptions<WebFormContext> options) : base(options) {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(person =>
        {
            person.HasKey(p => p.Id);
            person.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            person.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            person.Property(p => p.IdentificationNumber).IsRequired().HasMaxLength(20);
            person.Property(p => p.Email).IsRequired().HasMaxLength(100);
            person.Property(p => p.IdentificationType).IsRequired().HasMaxLength(50);
            person.Property(p => p.CreationDate).IsRequired().HasDefaultValueSql("GETDATE()");
            person.Property(p => p.IdentificationNumberConcat).HasComputedColumnSql("[IdentificationNumber] + '-' + [IdentificationType]").ValueGeneratedOnAddOrUpdate();
            person.Property(p => p.FullNameConcat).HasComputedColumnSql("[FirstName] + ' ' + [LastName]").ValueGeneratedOnAddOrUpdate();
        });

        modelBuilder.Entity<User>(user =>
        {
            user.HasKey(u => u.Id);
            user.Property(u => u.Username).IsRequired().HasMaxLength(50);
            user.Property(u => u.Password).IsRequired().HasMaxLength(50);
            user.Property(u => u.CreationDate).IsRequired().HasDefaultValueSql("GETDATE()");
        });
    }
}