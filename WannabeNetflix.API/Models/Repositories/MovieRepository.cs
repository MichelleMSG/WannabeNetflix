using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WannabeNetflix.API.Models;
using WannabeNetflix.API.Models.Services;

namespace WannabeNetflix.API.Models.Repositories;

// Handles communication between the API and MongoDB
public class MovieRepository : IMovieRepository
{
    // Stores the movie collection from MongoDB
    private readonly IMongoCollection<Movie> _movies;

    // Stores the service used to upload movie files
    private readonly MovieFileService _movieFileService;

    // Connects to MongoDB and selects the movie collection
    public MovieRepository(
        IOptions<MovieDatabase> movieDatabase,
        MovieFileService movieFileService)
    {
        // Stores the movie file service for later use
        _movieFileService = movieFileService;

        // Creates a connection to MongoDB
        var mongoClient = new MongoClient(movieDatabase.Value.ConnectionString);

        // Selects the database
        var mongoDatabase = mongoClient.GetDatabase(movieDatabase.Value.DatabaseName);

        // Selects the movie collection
        _movies = mongoDatabase.GetCollection<Movie>(
            movieDatabase.Value.MoviesCollectionName);
    }

    // Gets all movies from the collection
    public async Task<List<Movie>> GetAllAsync()
    {
        return await _movies.Find(_ => true).ToListAsync();
    }

    // Finds a movie using its ID
    public async Task<Movie?> GetByIdAsync(string id)
    {
        return await _movies.Find(movie => movie.Id == id).FirstOrDefaultAsync();
    }

    // Inserts a new movie into the collection
    public async Task AddAsync(Movie movie)
    {
        await _movies.InsertOneAsync(movie);
    }

    // Updates an existing movie in the collection
    public async Task UpdateAsync(Movie movie)
    {
        await _movies.ReplaceOneAsync(
            existingMovie => existingMovie.Id == movie.Id,
            movie);
    }

    // Finds movies where the title contains the search text
    public async Task<List<Movie>> SearchAsync(string title)
    {
        return await _movies.Find(movie =>
            movie.Title.ToLower().Contains(title.ToLower())).ToListAsync();
    }

    // Uploads a movie file using the movie file service
    public async Task<string> UploadMovieFileAsync(
        string fileName,
        Stream fileStream)
    {
        return await _movieFileService.UploadAsync(fileName, fileStream);
    }

    // Downloads a movie file using the movie file service
    public async Task DownloadMovieFileAsync(string fileId, Stream destination)
    {
        await _movieFileService.DownloadAsync(fileId, destination);
    }
}