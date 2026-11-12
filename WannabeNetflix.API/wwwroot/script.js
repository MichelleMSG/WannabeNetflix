
// Stores all movies received from the API
let allMovies = [];

// Gets the movie list from the API
fetch("/movies")
    .then(response => response.json())
    .then(movies => {

        // Saves all movies so we can search through them later
        allMovies = movies;

        // Displays all movies when the website opens
        displayMovies(allMovies);
    });

// Displays movies in the movie list
function displayMovies(movies) {

    // Finds the movie list on the website
    const movieList = document.getElementById("movieList");

    // Clears the old movie list
    movieList.innerHTML = "";

    // Goes through every movie
    movies.forEach(movie => {

        // Creates a new list item
        const listItem = document.createElement("li");

        // Creates a button for the movie title
        const movieButton = document.createElement("button");

        // Sets the button text to the movie title
        movieButton.textContent = movie.title;

        // Runs when someone clicks the movie
        movieButton.addEventListener("click", function () {

            // Displays the movie title
            document.getElementById("movieTitle").textContent = movie.title;

            // Displays the release year
            document.getElementById("movieYear").textContent = "Release year: " + movie.releaseYear;

            // Displays the genre
            document.getElementById("movieGenre").textContent = "Genre: " + movie.genre;

            // Displays the description
            document.getElementById("movieDescription").textContent = movie.description;

            // Finds the video player
            const moviePlayer = document.getElementById("moviePlayer");

            // Loads the selected movie's video
            moviePlayer.src = "/movies/" + movie.id + "/video";
        });

        // Adds the button to the list item
        listItem.appendChild(movieButton);

        // Adds the movie to the movie list
        movieList.appendChild(listItem);
    });
}

// Finds the search button
const searchButton = document.getElementById("searchButton");

// Runs when the search button is clicked
searchButton.addEventListener("click", function () {

    // Gets the text entered into the search field
    const searchText = document.getElementById("searchInput").value.toLowerCase();

    // Finds movies with titles matching the search
    const matchingMovies = allMovies.filter(movie =>
        movie.title.toLowerCase().includes(searchText)
    );

    // Displays only the matching movies
    displayMovies(matchingMovies);
});

