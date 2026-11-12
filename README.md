# WannabeNetflix

## Project description

WannabeNetflix is a simple movie streaming proof of concept (PoC). The application allows users to import movies, store movie metadata and video files in MongoDB, search for movies, select a movie and stream it through a web frontend.

## Technologies

- .NET 10 WebAPI
- MongoDB 7
- MongoDB GridFS for video file storage
- Docker
- Docker Compose
- Swagger / OpenAPI
- HTML, CSS and JavaScript

## Requirements

To run the application, you need:

- Docker Desktop with Docker Compose

## Setup instructions

1. Download or clone the project.
2. Open a terminal in the project folder containing `compose.yaml`.
3. Start the application using:

```bash
docker compose up -d --build
```

This builds and starts the API and MongoDB containers.

To stop the containers, run:

```bash
docker compose down
```

The MongoDB data is stored in a Docker volume named `mongo`, so stopping the containers does not remove the stored database data.

## Accessing the application

Once the containers are running, the application can be accessed at:

- **Web frontend:** http://localhost:8080
- **Swagger UI:** http://localhost:8080/swagger/index.html
- **OpenAPI documentation:** http://localhost:8080/openapi/v1.json

## Importing movies

Movies can be imported through Swagger.

1. Open the Swagger UI.
2. Find the `POST /movies` endpoint.
3. Enter the movie metadata.
4. Execute the request.

After creating a movie, its video file can be uploaded using:

`POST /movies/{id}/video`

Use the movie's ID from the previous response and select an MP4 video file.

## Movie features

The application supports:

- Importing movies into MongoDB.
- Storing movie metadata.
- Storing video files using MongoDB GridFS.
- Retrieving movie metadata through the API.
- Searching movies by title.
- Selecting movies in the web frontend.
- Streaming selected movies through the browser's video player.

## API endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/movies` | Retrieves all movies. |
| GET | `/movies/{id}` | Retrieves a movie by ID. |
| GET | `/movies/search?title=` | Searches movies by title. |
| POST | `/movies` | Imports a movie's metadata. |
| POST | `/movies/{id}/video` | Uploads a movie video. |
| GET | `/movies/{id}/video` | Streams a movie video. |

## Docker containers

The application uses Docker Compose to run two services:

- **mongohost:** Runs MongoDB 7 and stores database data in a persistent volume.
- **server-movie:** Builds and runs the .NET 10 WebAPI.

The API connects to MongoDB using the Docker service name `mongohost`.