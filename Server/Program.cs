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
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
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
    var db = scope.ServiceProvider.GetRequiredService<FractureDbContext>();
    db.Database.Migrate();
}

app.Run();
