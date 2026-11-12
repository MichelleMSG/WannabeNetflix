
using System.Text.Json.Serialization;
using WannabeNetflix.API.Models;
using WannabeNetflix.API.Models.Repositories;
using MongoDB.Driver;
using WannabeNetflix.API.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Adds OpenAPI documentation for the API
builder.Services.AddOpenApi();

// Configures how JSON data is handled
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});

// Loads the MongoDB settings from appsettings.json
builder.Services.Configure<MovieDatabase>(
    builder.Configuration.GetSection("MovieDatabaseSettings"));

// Registers the movie repository so the API can use it
builder.Services.AddSingleton<IMovieRepository, MovieRepository>();

// Registers the MongoDB database so other services can use it
builder.Services.AddSingleton<IMongoDatabase>(serviceProvider =>
{
    // Gets the MongoDB settings from the application configuration
    var settings = serviceProvider.GetRequiredService<
        Microsoft.Extensions.Options.IOptions<MovieDatabase>>().Value;

    // Creates a connection to MongoDB
    var client = new MongoClient(settings.ConnectionString);

    // Selects the database
    return client.GetDatabase(settings.DatabaseName);
});

// Registers the movie file service so the API can upload movie files
builder.Services.AddSingleton<MovieFileService>();

var app = builder.Build();

// Allows the browser to find index.html when visiting the website root
app.UseDefaultFiles();

// Makes files inside wwwroot available to the browser
app.UseStaticFiles();

// Makes the OpenAPI documentation available
app.MapOpenApi();

// Makes the Swagger UI available
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "WannabeNetflix API v1");
});

// Redirects HTTP requests to HTTPS when HTTPS is configured
app.UseHttpsRedirection();

// Gets all movies from the database
app.MapGet("/movies", async (IMovieRepository repository) =>
{
    return await repository.GetAllAsync();
});

// Gets a specific movie using its ID
app.MapGet("/movies/{id}", async (string id, IMovieRepository repository) =>
{
    // Finds the movie using its ID
    var movie = await repository.GetByIdAsync(id);

    // Returns 404 if the movie does not exist
    return movie is null ? Results.NotFound() : Results.Ok(movie);
});

// Searches for movies by title
app.MapGet("/movies/search",
    async (string title, IMovieRepository repository) =>
    {
        return await repository.SearchAsync(title);
    });

// Adds a new movie to the database
app.MapPost("/movies", async (Movie movie, IMovieRepository repository) =>
{
    // Saves the movie in MongoDB
    await repository.AddAsync(movie);

    // Returns the created movie and its location
    return Results.Created($"/movies/{movie.Id}", movie);
});

// Uploads a video file for an existing movie
app.MapPost("/movies/{id}/video", async (
    string id,
    [FromForm] IFormFile file,
    IMovieRepository repository) =>
{
    // Finds the movie using its ID
    var movie = await repository.GetByIdAsync(id);

    // Checks if the movie exists
    if (movie is null)
        return Results.NotFound("Movie not found.");

    // Checks if a file was provided
    if (file.Length == 0)
        return Results.BadRequest("No file was uploaded.");

    // Opens the uploaded file
    using var fileStream = file.OpenReadStream();

    // Uploads the video to MongoDB GridFS
    var fileId = await repository.UploadMovieFileAsync(
        file.FileName,
        fileStream);

    // Saves the GridFS file ID on the movie
    movie.FileId = fileId;

    // Updates the movie in MongoDB
    await repository.UpdateAsync(movie);

    // Returns the updated movie
    return Results.Ok(movie);
})
.DisableAntiforgery();

// Downloads a video file for an existing movie
app.MapGet("/movies/{id}/video", async (
    string id,
    IMovieRepository repository) =>
{
    // Finds the movie using its ID
    var movie = await repository.GetByIdAsync(id);

    // Checks if the movie exists and has a video file
    if (movie is null || string.IsNullOrEmpty(movie.FileId))
        return Results.NotFound("Video not found.");

    // Creates a stream to hold the downloaded video
    var stream = new MemoryStream();

    // Downloads the video from MongoDB GridFS
    await repository.DownloadMovieFileAsync(movie.FileId, stream);

    // Resets the stream position so the video can be read
    stream.Position = 0;

    // Returns the video file as an MP4
    return Results.File(stream, "video/mp4");
});

// Starts the API and web frontend
app.Run();
