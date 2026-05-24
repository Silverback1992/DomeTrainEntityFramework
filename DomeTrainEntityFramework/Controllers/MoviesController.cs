using DomeTrainEntityFramework.Data;
using DomeTrainEntityFramework.Models;
using DomeTrainEntityFramework.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DomeTrainEntityFramework.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : Controller
{
    private readonly MoviesContext _context;

    public MoviesController(MoviesContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Movies.ToListAsync());
    }

    [HttpGet("until-age/{ageRating}")]
    [ProducesResponseType(typeof(List<MovieTitle>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUntilAge([FromRoute] AgeRating ageRating)
    {
        var movies = await _context.Movies
            .Where(m => m.AgeRating <= ageRating)
            .Select(m => new MovieTitle { Title = m.Title })
            .ToListAsync();

        return Ok(movies);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        // Queries database, returns first match, null if no match found
        // var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id);
        // Similar to FirstOrDefaultAsync, but throws if more than one match found
        // var movie = await _context.Movies.SingleOrDefaultAsync(x => x.Id == id);
        // Servers match from memory if already fetched, otherwise queries database. Returns null if no match found
        // var movie = await _context.Movies.FindAsync(id);

        var movie = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Identifier == id);

        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie);
    }

    [HttpGet("by-year/{year:int}")]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByYear([FromRoute] int year)
    {
        var movies = await _context.Movies.Where(x => x.ReleaseDate.Year == year).ToListAsync();
        return Ok(movies);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] Movie movie)
    {
        await _context.Movies.AddAsync(movie);

        // movie has not been assigned an Id yet, so it will be 0
        await _context.SaveChangesAsync();
        // After SaveChangesAsync, movie will have its Id assigned by the database

        return CreatedAtAction(nameof(Get), new { id = movie.Identifier }, movie);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Movie movie)
    {
        var existingMovie = await _context.Movies.FindAsync(id);

        if (existingMovie == null)
        {
            return NotFound();
        }

        existingMovie.Title = movie.Title;
        existingMovie.ReleaseDate = movie.ReleaseDate;
        existingMovie.Synopsis = movie.Synopsis;

        await _context.SaveChangesAsync();

        return Ok(existingMovie);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int id)
    {
        var existingMovie = await _context.Movies.FindAsync(id);

        if (existingMovie == null)
        {
            return NotFound();
        }

        _context.Movies.Remove(existingMovie);

        await _context.SaveChangesAsync();

        return Ok();
    }
}
