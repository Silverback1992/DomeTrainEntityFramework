using DomeTrainEntityFramework.Models.Enums;

namespace DomeTrainEntityFramework.Models;

public class Movie
{
    public int Identifier { get; set; }
    public string? Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? Synopsis { get; set; }
    public AgeRating AgeRating { get; set; }

    public Person Director { get; set; }
    public ICollection<Person> Actors { get; set; }

    public Genre Genre { get; set; }
    public int MainGenreId { get; set; }
}

//[Table("Pictures")]
//public class Movie
//{
//    [Key]
//    public int Identifier { get; set; }
//    [MaxLength(128)]
//    [Column(TypeName = "varchar")]
//    [Required]
//    public string? Title { get; set; }
//    [Column(TypeName = "date")]
//    public DateTime ReleaseDate { get; set; }
//    [Column("Plot", TypeName = "varchar(max)")]
//    public string? Synopsis { get; set; }
//}
