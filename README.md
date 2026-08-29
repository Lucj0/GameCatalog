# GameCatalog API

A RESTful Web API for cataloging games and movies, built with ASP.NET Core and Entity Framework Core.

## Overview

This is a learning project built as part of a self-taught path toward a software development engineer in test (SDET) role. It implements a database-backed REST API with full CRUD operations across two resources, along with server-side input validation focused on getting the fundamentals right: clean controller design, dependency injection, a data layer built with Entity Framework Core, and validated request handling.

## Tech Stack

- **C#** on **.NET 10**
- **ASP.NET Core** — Web API framework
- **Entity Framework Core** — ORM for data access
- **SQLite** — lightweight relational database
- **EF Core Migrations** — schema versioning

## Features

- Full CRUD (Create, Read, Update, Delete) for **Games** and **Movies**
- Database persistence via EF Core and SQLite
- Server-side input validation using data annotations (required fields, string length limits, numeric ranges)
- Automatic `400 Bad Request` responses with detailed validation errors for invalid input
- Correct RESTful status codes (`201 Created` with `Location` header on create, `204 No Content` on update/delete, `404 Not Found` for missing resources)

## API Endpoints

### Games

| Method | Endpoint      | Description              |
|--------|---------------|--------------------------|
| GET    | `/Games`      | Get all games            |
| GET    | `/Games/{id}` | Get a single game by ID  |
| POST   | `/Games`      | Create a new game        |
| PUT    | `/Games/{id}` | Update an existing game  |
| DELETE | `/Games/{id}` | Delete a game            |

### Movies

| Method | Endpoint       | Description               |
|--------|----------------|---------------------------|
| GET    | `/Movies`      | Get all movies            |
| GET    | `/Movies/{id}` | Get a single movie by ID  |
| POST   | `/Movies`      | Create a new movie        |
| PUT    | `/Movies/{id}` | Update an existing movie  |
| DELETE | `/Movies/{id}` | Delete a movie            |

## Getting Started

### Prerequisites

- The `dotnet-ef` tool: `dotnet tool install --global dotnet-ef`

### Running the project

```bash
# Clone the repository
git clone https://github.com/Lucj0/GameCatalog.git
cd GameCatalog

# Restore dependencies
dotnet restore

# Apply migrations to create the database
dotnet ef database update

# Run the API
dotnet run
```

Once running, the console will print the local address the API is listening on (e.g. `http://localhost:5062`). Endpoints can be exercised using the included `GameCatalog.http` file (with the VS Code REST Client extension) or any HTTP client