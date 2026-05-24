using DomeTrainEntityFramework.Data.ValueConverters;
using DomeTrainEntityFramework.Models;
using DomeTrainEntityFramework.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomeTrainEntityFramework.Data.EntityMapping;

public class MovieMapping : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Pictures")
            .HasQueryFilter(m => m.ReleaseDate.Year >= 1990) // Global query filter, applies to all queries for this entity
            .HasKey(m => m.Identifier);

        builder.Property(m => m.Title)
            .HasMaxLength(128)
            .HasColumnType("varchar")
            .IsRequired();

        builder.Property(m => m.ReleaseDate)
            .HasColumnType("char(8)")
            .HasConversion(new DateTimeToChar8Converter());

        builder.Property(m => m.Synopsis)
            .HasColumnType("varchar(max)")
            .HasColumnName("Plot");

        builder.Property(m => m.AgeRating)
            .HasConversion<string>()
            .HasColumnType("varchar(32)");

        builder.OwnsOne(m => m.Director)
            .ToTable("Directors");

        builder.OwnsMany(m => m.Actors)
            .ToTable("Actors");

        builder
            .HasOne(m => m.Genre)
            .WithMany(g => g.Movies)
            .HasPrincipalKey(g => g.Id)
            .HasForeignKey(m => m.MainGenreId);

        // Seed data

        builder.HasData(
            new Movie
            {
                Identifier = 1,
                Title = "The Shawshank Redemption",
                ReleaseDate = new DateTime(1994, 9, 22),
                Synopsis = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                MainGenreId = 1,
                AgeRating = AgeRating.Adult,
            });

        builder.OwnsOne(m => m.Director)
            .HasData(new { MovieIdentifier = 1, FirstName = "Frank", LastName = "Darabont" });

        builder.OwnsMany(m => m.Actors)
            .HasData(new { MovieIdentifier = 1, Id = 1, FirstName = "Tim", LastName = "Robbins" },
                     new { MovieIdentifier = 1, Id = 2, FirstName = "Morgan", LastName = "Freeman" });
    }
}
