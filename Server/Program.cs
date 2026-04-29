using System.ClientModel;
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
using Fracture.Server.Modules.Shared.NameGenerators;
using Fracture.Server.Modules.Users.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<NameGeneratorConfig>(builder.Configuration.GetSection("NameGenerator"));
builder.Services.Configure<AIBackendConfiguration>(builder.Configuration.GetSection("AiBackend"));

builder.Configuration.AddJsonFile(
    "Config/NameGenerator.json",
    optional: false,
    reloadOnChange: true
); // Przeniesienie NameGenerator a appsettings do folderu Config

var r = new RandomProvider(42);
builder.Services.AddSingleton<RandomProvider>(r);

builder.Services.AddSingleton<INameGenerator, MarkovNameGenerator>();
builder.Services.AddSingleton<IItemGenerator, ItemGenerator>();
builder.Services.AddSingleton<PrefixesGenerator>();
builder.Services.AddSingleton<VersionInfoProvider>();
builder.Services.AddSingleton<IMapRepository, InMemoryMapRepository>();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IItemsRepository, ItemsRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MovementService>();

builder.Services.AddTransient<IWorldGenerationService, WorldGenerationService>();
builder.Services.AddSingleton<IMapParameterSelectorService, MapParameterSelectorService>();
builder.Services.AddSingleton<ILocationGroupGeneratorService, LocationGroupGeneratorService>();
builder.Services.AddSingleton<ISubMapAssignmentService, SubMapAssignmentService>();
builder.Services.AddSingleton<IMapGeneratorService, MapGeneratorService>();
builder.Services.AddSingleton<MapParametersReader>();
builder.Services.AddSingleton<ILocationGeneratorService, WeightedLocationGeneratorService>();
builder.Services.AddSingleton<ILocationWeightGeneratorService, LocationBiomeWeightGenService>();

builder.Services.AddSingleton(typeof(IFloodFillService<>), typeof(FloodFillService<>));

builder.Services.AddFeatureManagement();

builder.Services.AddSingleton<MapDataImportService>();
builder.Services.AddSingleton<MapManagerService>();

builder.Services.AddSingletonIfFeatureEnabled<IChatClient>(
    FeatureFlags.USE_AI,
    sp =>
    {
        var ai = sp.GetRequiredService<IOptions<AIBackendConfiguration>>().Value;
        var credential = new ApiKeyCredential(
            ai.ApiKey ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? string.Empty
        );

        var options = new OpenAIClientOptions
        {
            Endpoint = new Uri(ai.EndpointUrl ?? string.Empty),
        };

        return new OpenAIClient(credential, options).GetChatClient(ai.Model).AsIChatClient();
    }
);
builder.Services.AddSingletonIfFeatureEnabled<SingleResponseProvider>(FeatureFlags.USE_AI);

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

builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", true);
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

using (var scope = app.Services.CreateScope())
{
    var mapManagerService = scope.ServiceProvider.GetRequiredService<MapManagerService>();
    await mapManagerService.GenerateWorldAsync();

    var db = scope.ServiceProvider.GetRequiredService<FractureDbContext>();
    db.Database.Migrate();
}

app.Run();
