# GameCatalog API

[![CI](https://github.com/Lucj0/GameCatalog/actions/workflows/ci.yml/badge.svg)](https://github.com/Lucj0/GameCatalog/actions/workflows/ci.yml)

A RESTful Web API for cataloging games and movies, built with ASP.NET Core and Entity Framework Core, with a comprehensive automated test suite spanning unit and integration layers, run in CI on every push.

## Overview

This project implements a database-backed REST API with full CRUD across two resources and server-side validation. The primary focus is the test suite: unit tests covering controller logic in isolation, and integration tests exercising the full HTTP pipeline, including behaviors (validation, over-posting protection) that can only be verified end-to-end.

## Tech Stack

- **C# on .NET 10**
- **ASP.NET Core** — Web API framework
- **Entity Framework Core** — ORM for data access
- **SQLite** — lightweight relational database
- **EF Core Migrations** — schema versioning
- **xUnit** — unit and integration testing
- **GitHub Actions** — CI (builds and runs the full suite on every push)

## Features

- Full CRUD (Create, Read, Update, Delete) for Games and Movies
- Database persistence via EF Core and SQLite
- Server-side input validation using data annotations (required fields, string length limits, numeric ranges)
- Automatic 400 Bad Request responses with detailed validation errors for invalid input
- DTO layer separating the API contract from persistence entities (guards against over-posting)
- Correct RESTful status codes (201 Created with Location header on create, 204 No Content on update/delete, 404 Not Found for missing resources)

## Testing

The suite is split into two layers, each covering what the other cannot.

**Unit tests** exercise controller logic in isolation. Controllers are constructed directly with an EF Core In-Memory context, bypassing the web pipeline — fast, and covering every branch of every action (found / not-found paths, full CRUD).

**Integration tests** exercise the full HTTP pipeline. Using `WebApplicationFactory`, the entire app boots in-memory and tests send real HTTP requests through real routing, model binding, and validation. The database is overridden with in-memory SQLite (a real relational engine), and writes are verified through a fresh dependency-injection scope, so persistence is checked against the store rather than a cached object.

Integration tests also cover behavior that lives in the MVC pipeline and cannot be reached by unit tests:

- **Validation** — malformed requests return `400 Bad Request` (missing required fields, out-of-range values)
- **Over-posting protection** — a client-supplied `Id` is ignored rather than honored

### Running the tests

```bash
dotnet test
```

Run a single class or test with a filter:

```bash
dotnet test --filter "FullyQualifiedName~GamesIntegrationTests"
```

## API Endpoints

### Games

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /games | Get all games |
| GET | /games/{id} | Get a single game by ID |
| POST | /games | Create a new game |
| PUT | /games/{id} | Update an existing game |
| DELETE | /games/{id} | Delete a game |

### Movies

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /movies | Get all movies |
| GET | /movies/{id} | Get a single movie by ID |
| POST | /movies | Create a new movie |
| PUT | /movies/{id} | Update an existing movie |
| DELETE | /movies/{id} | Delete a movie |

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

Once running, the console will print the local address the API is listening on (e.g. `http://localhost:5062`). Endpoints can be exercised using the included `GameCatalog.http` file (with the VS Code REST Client extension) or any HTTP client.