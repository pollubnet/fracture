using Fracture.Server.Components;
using Fracture.Server.Modules.AI.Models;
using Fracture.Server.Modules.AI.Services;
using Fracture.Server.Modules.Database;
using Fracture.Server.Modules.FloodFill;
using Fracture.Server.Modules.Items.Models;
using Fracture.Server.Modules.Items.Services;
using Fracture.Server.Modules.MapGenerator;
using Fracture.Server.Modules.MapGenerator.Services;
using Fracture.Server.Modules.MapGenerator.Services.TownGen;
using Fracture.Server.Modules.Shared;
using Fracture.Server.Modules.Shared.Configuration;
using Fracture.Server.Modules.Shared.ImageChangers;
using Fracture.Server.Modules.Shared.NameGenerators;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<NameGeneratorConfig>(builder.Configuration.GetSection("NameGenerator"));
builder.Services.Configure<AIBackendConfiguration>(builder.Configuration.GetSection("AiBackend"));

builder.Services.AddSingleton<INameGenerator, MarkovNameGenerator>();
builder.Services.AddSingleton<IItemGenerator, ItemGenerator>();
builder.Services.AddSingleton<PrefixesGenerator>();
builder.Services.AddSingleton<VersionInfoProvider>();
builder.Services.AddSingleton<IMapRepository, InMemoryMapRepository>();

builder.Services.AddScoped<BackgroundImageChanger>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IItemsRepository, ItemsRepository>();
builder.Services.AddTransient<IWorldGenerationService, WorldGenerationService>();
builder.Services.AddSingleton<IMapParameterSelectorService, MapParameterSelectorService>();
builder.Services.AddSingleton<ILocationGroupGeneratorService, LocationGroupGeneratorService>();
builder.Services.AddScoped<ISubMapAssignmentService, SubMapAssignmentService>();
builder.Services.AddSingleton<IMapGeneratorService, MapGeneratorService>();
builder.Services.AddSingleton<MapParametersReader>();
builder.Services.AddSingleton<ILocationGeneratorService, WeightedLocationGeneratorService>();
builder.Services.AddSingleton<ILocationWeightGeneratorService, LocationBiomeWeightGenService>();

builder.Services.AddHangfire(config => config.UseInMemoryStorage()); // lub .UseSqlServerStorage(connectionString)
builder.Services.AddSingleton(typeof(IFloodFillService<>), typeof(FloodFillService<>));

builder.Services.AddHangfireServer();
builder.Services.AddFeatureManagement();

builder.Services.AddSingleton<MapDataImportService>();
builder.Services.AddSingleton<MapManagerService>();

builder.Services.AddSingletonIfFeatureEnabled<
    IAIInstructionProvider,
    OpenAICompatibleInstructionProvider
>(FeatureFlags.USE_AI);

builder
    .Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

builder.Services.AddDbContext<FractureDbContext>(options =>
{
    options.UseSqlite("Data Source=fracture.db");
    options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
});

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
    app.UseExceptionHandler("/Error", true);
}

app.UseHangfireDashboard();
app.UseHangfireServer();

app.UseStaticFiles();
app.UseAuthorization();

app.UseRouting();
app.UseAntiforgery();
app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

using (var scope = app.Services.CreateScope())
{
    var mapManagerService = scope.ServiceProvider.GetRequiredService<MapManagerService>();
    await mapManagerService.InitializeAndScheduleMapsAsync();
    var db = scope.ServiceProvider.GetRequiredService<FractureDbContext>();
    db.Database.Migrate();
}

app.Run();
