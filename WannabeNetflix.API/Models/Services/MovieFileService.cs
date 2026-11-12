using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace WannabeNetflix.API.Models.Services;

public class MovieFileService
{
    // Stores the GridFS bucket used to upload and download movie files
    private readonly GridFSBucket _bucket;

    // Sets up GridFS using the MongoDB database
    public MovieFileService(IMongoDatabase database)
    {
        _bucket = new GridFSBucket(database);
    }

    // Uploads a movie file to MongoDB and returns its file ID
    public async Task<string> UploadAsync(string fileName, Stream fileStream)
    {
        // Uploads the file stream to GridFS and gets its ID
        var fileId = await _bucket.UploadFromStreamAsync(fileName, fileStream);

        // Converts the file ID to a string so it can be stored with the movie
        return fileId.ToString();
    }

    // Downloads a movie file from GridFS
    public async Task DownloadAsync(string fileId, Stream destination)
    {
        // Converts the string ID back into MongoDB's ObjectId format
        var objectId = ObjectId.Parse(fileId);

        // Downloads the file into the destination stream
        await _bucket.DownloadToStreamAsync(objectId, destination);
    }
}