# Transaction Workflow Engine

A .NET 8 Web API that manages transactions through a dynamic, data-driven workflow (Jira-like status transitions).

## Tech Stack

- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core + SQL Server
- Swagger / OpenAPI
- Docker Compose (for SQL Server)

## Assignment Goals Covered

### Mandatory

- Dynamic workflow with DB-driven statuses and allowed transitions (no enums)
- APIs:
  - `POST /transactions`
  - `GET /transactions/{id}`
  - `POST /transactions/{id}/transition`
  - `GET /transactions/{id}/available-transition`
- Invalid transitions are rejected with meaningful errors
- Status change logic is outside controllers
- Controllers do not write directly to DB

### Bonus Implemented

- Transaction status history table and endpoint:
  - `GET /transactions/{id}/history`
- Concurrency support via EF optimistic concurrency (`RowVersion`)
- Conflict mapping to HTTP `409 Conflict` for concurrency collisions

### Not Implemented (Bonus)

- Workflow caching of transitions
- Admin endpoints for adding statuses/transitions
- Automated unit/integration tests

## Project Structure

- `TransactionWorkflowEngine/Controllers` — API endpoints
- `TransactionWorkflowEngine/Handlers` — orchestration layer (application flow)
- `TransactionWorkflowEngine/Services` — domain/data services
- `TransactionWorkflowEngine/Data` — EF `DbContext`
- `TransactionWorkflowEngine/Models` — entities
- `TransactionWorkflowEngine/Migrations` — schema + seed data
- `docker-compose.yml` — SQL Server container

## Workflow Seed Data

Initial statuses and transitions are seeded in EF migration:

`CREATED → VALIDATED → PROCESSING → COMPLETED`

and retry path:

`VALIDATED → FAILED → VALIDATED`

## Prerequisites

- .NET SDK 8.0+
- Docker Desktop (or Docker Engine with Compose)
- (Optional) `dotnet-ef` CLI if you need to apply migrations manually

## How to Run

### 1) Start SQL Server in Docker

From repo root:

```bash
docker compose up -d
```

This starts SQL Server on `localhost:1433` with credentials from `docker-compose.yml`.

### 2) Verify connection string

`TransactionWorkflowEngine/appsettings.Development.json` already points to:

```json
"ConnectionStrings": {
  "sqlServer": "Server=localhost,1433;Database=TransactionWorkflowDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;"
}
```

### 3) Apply database migrations

From repo root:

```bash
dotnet ef database update --project TransactionWorkflowEngine/TransactionWorkflowEngine.csproj
```

If `dotnet ef` is missing:

```bash
dotnet tool install --global dotnet-ef
```

### 4) Run the API

```bash
dotnet run --project TransactionWorkflowEngine/TransactionWorkflowEngine.csproj
```

### 5) Open Swagger

- `https://localhost:7135/swagger`
- or `http://localhost:5140/swagger`

(Exact URLs come from `Properties/launchSettings.json`.)

## Build

From repo root:

```bash
dotnet build TransactionWorkflowEngine.sln -c Release
```

## API Endpoints

Base path: `/transactions`

### Create transaction

- **POST** `/transactions`
- Request body: none
- Response: `201 Created`

### Get transaction by id

- **GET** `/transactions/{id}`
- Response: `200 OK` or `404 Not Found`

### Get available transitions

- **GET** `/transactions/{id}/available-transition`
- Response: `200 OK` or `404 Not Found`

### Transition transaction

- **POST** `/transactions/{id}/transition`
- Request body:

```json
{
  "toStatusId": 3,
  "reason": "Validation passed"
}
```

- Response:
  - `200 OK` when transition succeeds
  - `400 Bad Request` for invalid transition
  - `404 Not Found` for unknown transaction
  - `409 Conflict` for optimistic concurrency conflicts

### Get transition history

- **GET** `/transactions/{id}/history`
- Response: `200 OK` or `404 Not Found`

## Error Handling

Global error middleware maps exceptions to consistent JSON error payloads:

- `InvalidOperationException` → `400 Bad Request`
- `DbUpdateConcurrencyException` → `409 Conflict`
- `KeyNotFoundException` → `404 Not Found`
- unhandled exception → `500 Internal Server Error`

## Design Decisions

- **Layered architecture**: Controllers are thin; handler/service layers contain workflow logic.
- **Data-driven workflow**: allowed transitions are loaded from DB (`TransactionStatusTransitions`).
- **Atomic transition update**: status update + history insert are persisted in one `SaveChanges` call to avoid partial state.
- **Optimistic concurrency**: `RowVersion` on `Transaction` prevents silent lost updates in concurrent writes.

## Tradeoffs

- Kept implementation focused on assignment core scope over operational extras.
- Chose readability and clear boundaries over advanced infrastructure patterns.
- Did not add caching/admin management/tests yet to keep delivery concise.

## Useful Commands

Stop DB container:

```bash
docker compose down
```

Stop DB + remove volume (clean reset):

```bash
docker compose down -v
```

## Notes for Reviewer

- SQL Server is expected to run locally in Docker.
- API is intended to run locally via `dotnet run`.
- Seeded workflow data is included in migrations.
