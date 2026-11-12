using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WannabeNetflix.API.Models;

// Represents a movie and its information
public class Movie
{
    // MongoDB uses this as the movie's unique ID
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    // Movie information
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Genre { get; set; } = "";
    public int ReleaseYear { get; set; }

// Stores the ID of the movie's video file in GridFS
    public string FileId { get; set; } = "";
}