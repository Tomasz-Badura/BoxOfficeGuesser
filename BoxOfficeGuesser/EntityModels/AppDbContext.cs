using Microsoft.EntityFrameworkCore;

using BoxOfficeGuesser.Model;

namespace BoxOfficeGuesser.EntityModels;

public class AppDbContext : DbContext
{
    public DbSet<Score> Scores { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Score>(entity =>
        {
            _ = entity.HasKey(e => e.ID);
            _ = entity.Property(e => e.Date).IsRequired();
            _ = entity.Property(e => e.Username).IsRequired().HasMaxLength(30);
            _ = entity.Property(e => e.Points).IsRequired();
            _ = entity.Property(e => e.Difficulty).IsRequired();
        });
    }
}