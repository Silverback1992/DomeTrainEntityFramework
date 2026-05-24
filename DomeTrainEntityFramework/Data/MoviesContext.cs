using DomeTrainEntityFramework.Data.EntityMapping;
using DomeTrainEntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace DomeTrainEntityFramework.Data;

public class MoviesContext : DbContext
{
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=MoviesDb;User ID=sa;Password=MySaPassword123;TrustServerCertificate=true;");
        // Not proper logging
        optionsBuilder.LogTo(Console.WriteLine);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GenreMapping());
        modelBuilder.ApplyConfiguration(new MovieMapping());

        //base.OnModelCreating(modelBuilder);
    }
}
