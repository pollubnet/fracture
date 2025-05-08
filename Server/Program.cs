using Fracture.Server.Components;
using Fracture.Server.Modules.AI.Models;
using Fracture.Server.Modules.AI.Services;
using Fracture.Server.Modules.Database;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Items.Services;
using Fracture.Server.Modules.MapGenerator.Services;
using Fracture.Server.Modules.MapGenerator.Services.TownGen;
using Fracture.Server.Modules.Shared;
using Fracture.Server.Modules.Shared.Configuration;
using Fracture.Server.Modules.Shared.ImageChangers;
using Fracture.Server.Modules.Shared.NameGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<NameGeneratorConfig>(builder.Configuration.GetSection("NameGenerator"));
builder.Services.Configure<AIBackendConfiguration>(builder.Configuration.GetSection("AiBackend"));

builder.Services.AddSingleton<INameGenerator, MarkovNameGenerator>();
builder.Services.AddSingleton<IItemGenerator, ItemGenerator>();
builder.Services.AddSingleton<PrefixesGenerator>();
builder.Services.AddSingleton<VersionInfoProvider>();
builder.Services.AddSingleton<IMapGeneratorService, MapGeneratorService>();
builder.Services.AddSingleton<ILocationWeightGeneratorService, LocationBiomeWeightGenService>();
builder.Services.AddSingleton<ILocationGeneratorService, WeightedLocationGeneratorService>();
builder.Services.AddSingleton<MapParametersService>();
builder.Services.AddSingleton<DockerService>();

builder.Services.AddScoped<BackgroundImageChanger>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IItemsRepository, ItemsRepository>();

builder.Services.AddFeatureManagement(
    builder.Configuration.GetSection(FeatureFlags.CONFIG_SECTION)
);

builder.Services.AddSingletonIfFeatureEnabled<
    IAIInstructionProvider,
    OpenAICompatibleInstructionProvider
>(FeatureFlags.USE_AI);

builder
    .Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

var dockerService = new DockerService(
    LoggerFactory.Create(b => b.AddConsole()).CreateLogger<DockerService>()
);

try
{
    await dockerService.EnsurePostgresRunningAsync();

    var connectionString = builder
        .Configuration.GetConnectionString("DefaultConnection")
        .Replace("{DYNAMIC_PORT}", dockerService.AssignedHostPort.ToString() ?? "5432");

    builder.Services.AddDbContext<FractureDbContext>(options =>
        options.UseNpgsql(connectionString)
    );
}
catch (Exception e)
{
    Console.WriteLine($"Failed to initialize database: {e.Message}");
    throw;
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<FractureDbContext>();
    await dbContext.Database.MigrateAsync();
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    throw;
}

app.UseStaticFiles();
app.UseAuthorization();

app.UseRouting();
app.UseAntiforgery();
app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.Run();
