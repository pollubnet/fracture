# Repository guide

## Project and runtime

- This is a .NET 10 ASP.NET Core / Blazor game application. `Fracture.sln` currently contains only `Server/Fracture.Server.csproj`; `Tests/Fracture.Tests/` exists but has no project or source files, so there is no checked-in automated test suite yet.
- `Server/Program.cs` is the composition root: it registers the application services, EF Core with SQLite (`fracture.db`), feature management, AI integration, and interactive Razor components. On startup it generates a world and applies database migrations before serving requests.
- The game UI is under `Server/Components` (Razor components and code-behind). Feature/domain logic is grouped under `Server/Modules`: map/noise generation, pathfinding, users/movement, items, database repositories, UI state, and optional AI.
- A typical world-generation flow is `MapManagerService` -> `WorldGenerationService` -> parameter selection and map generation -> submap assignment; maps are held in `InMemoryMapRepository`. Movement and user state are managed by scoped services, while generated world/map state is registered as singleton state.
- Map and item generation inputs are data-driven JSON under `Server/Config`. Map parameter JSON is deserialized into typed models with string-enum and color converters; keep JSON values consistent with those models and enum names.

## Commands

Run from the repository root unless noted:

```sh
dotnet tool restore
dotnet restore Fracture.sln
dotnet build --no-restore Fracture.sln
dotnet run --project Server/Fracture.Server.csproj
dotnet csharpier check .
```

- The CI workflow (`.github/workflows/pr.yml`) restores tools, checks CSharpier formatting, restores the solution, then builds it. It does not run tests.
- The SDK is pinned in `global.json` to 10.0.0 with latest-major roll-forward. CI installs .NET 10.
- Local run configuration is in `Server/Properties/launchSettings.json`; the HTTP profile uses `http://localhost:5124` and launches `/home`.
- There is no deploy or test command configured in the observed solution/CI. Do not assume `dotnet test` exercises application tests until a test project is added.

## Configuration and operational gotchas

- Configuration file paths in services are relative (for example, `Config/ItemParameters/itemgeneratorconfig.json` and `Config/MapParameters/`), and the app explicitly loads `Config/NameGenerator.json`. Run the app with `Server` as its working directory if a configuration file cannot be found; verify the actual working directory before changing path logic.
- SQLite uses the relative database file `fracture.db`; startup automatically calls `Database.Migrate()`. Database changes therefore affect the startup migration path as well as the EF model.
- AI is disabled by default (`FeatureFlags.UseAI` in `Server/appsettings.json`). Backend settings and any API key should be supplied through development secrets or environment configuration, not committed appsettings. `docs/ai.md` documents the AI setup and supports OpenAI-compatible endpoints.
- `RandomProvider` is registered as a singleton with seed `42` in `Program.cs`; map and item generation consume its shared `Random`. Changes to consumption/order can affect deterministic generated content and shared state, so avoid introducing independent random instances casually.
- The generated world is initialized once during application startup and retained in memory; it is not loaded from the SQLite database. `MapManagerService` swaps the current map on refresh.
- The pre-commit Husky task formats staged C# files with `dotnet csharpier format ${staged}`. Formatting is also enforced by CI, so format C# changes before finishing.

## Implementation conventions

- Follow the existing feature-folder layout under `Server/Modules/<Feature>` and keep interfaces near their implementations when a service has an interface. Register new services in `Server/Program.cs` with a lifetime compatible with their dependencies; in particular, singleton services must not capture scoped repositories or a `DbContext`.
- EF Core access is isolated behind repository interfaces in `Server/Modules/Database`; the scoped `FractureDbContext` is the persistence boundary. Keep entity relationships and indexes in `FractureDbContext.OnModelCreating` when changing persistence behavior.
- Use async methods for database/service operations as surrounding interfaces do. Blazor page/component event handlers coordinate scoped state and UI updates; event subscriptions and periodic work need lifecycle cleanup (see `GamePage` and `MovementService`).
- C# formatting is defined by CSharpier. The server `.editorconfig` requests underscore-prefixed private instance fields and uses PascalCase for interfaces/types; match the local pattern rather than introducing a new style.
- Keep static assets in `Server/wwwroot` and component-specific markup/styles with `Server/Components` where established.
