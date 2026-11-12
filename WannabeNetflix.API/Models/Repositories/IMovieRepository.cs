using WannabeNetflix.API.Models;

namespace WannabeNetflix.API.Models.Repositories;

// Defines the operations available for working with movies
public interface IMovieRepository
{
    // Gets all movies
    Task<List<Movie>> GetAllAsync();

    // Gets one movie using its ID
    Task<Movie?> GetByIdAsync(string id);

    // Adds a new movie
    Task AddAsync(Movie movie);

    // Updates an existing movie
    Task UpdateAsync(Movie movie);

    // Searches for movies by title
    Task<List<Movie>> SearchAsync(string title);

    // Uploads a movie file and returns its GridFS file ID
    Task<string> UploadMovieFileAsync(string fileName, Stream fileStream);
    
    // Downloads a movie file from MongoDB
    Task DownloadMovieFileAsync(string fileId, Stream destination);
}