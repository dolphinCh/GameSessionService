# GameSessionService

Small ASP.NET Core Web API for managing game sessions.

- .NET: .NET 9
- Language: C# 13

## Requirements

- .NET 9 SDK
- (Optional) SQL Server or SQLite depending on `appsettings.json` connection string

## Getting started

1. Restore and build:

```bash
dotnet restore
dotnet build
```

2. Configure the database connection string in `GameSessionService/appsettings.json`.

3. Apply EF Core migrations (from repository root):

```bash
dotnet ef database update --project Infrastructure --startup-project GameSessionService
```

4. Run the API:

```bash
dotnet run --project GameSessionService
```

The API will listen on the configured URLs (see console output).

## Endpoints

- `GET /session/{sessionId}`
  - Returns a session DTO or 404 if not found.
  - Example:

  ```bash
  curl http://localhost:5000/session/your-session-id
  ```

- `POST /session`
  - Start a session. Accepts a JSON body matching `StartSessionCommand`/`SessionDto`.
  - Example:

  ```bash
  curl -X POST http://localhost:5000/session -H "Content-Type: application/json" -d '{"playerId":"player1"}'
  ```

- `GET /diagnostics/perf-test?sessionId={id}&iterations={n}`
  - Runs a performance test that repeatedly queries a session and returns timing data.
  - Example:

  ```bash
  curl "http://localhost:5000/diagnostics/perf-test?sessionId=your-session-id&iterations=100"
  ```

## Notes

- The project uses MediatR for CQRS-style commands and queries.
- A `IPerformanceService` is registered and exposed via the diagnostics controller.
- The app uses in-memory caching; cache hits are exposed via the `X-Cache` response header on session GET.

## Development

- The API project is `GameSessionService`.
- Application logic lives under the `Application` project and infrastructure (EF Core) in `Infrastructure`.

## License

MIT
