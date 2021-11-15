using Microsoft.EntityFrameworkCore;
using RestWebApplication.Models;

namespace RestWebApplication.Services;

public class DataAccess : DbContext
{
    public DbSet<FileStorage> FileStorage { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileStorage>().HasKey(storage => storage.Id);
    }
}
