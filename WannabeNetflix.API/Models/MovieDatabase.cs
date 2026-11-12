namespace WannabeNetflix.API.Models;

// Stores the settings used to connect to MongoDB
public class MovieDatabase
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string MoviesCollectionName { get; set; } = null!;
}