using Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Data;

public class DatabaseContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<GameSession> GameSessions { get; set; }
    
    public DatabaseContext(DbContextOptions opts) : base(opts) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}