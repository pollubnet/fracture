using Fracture.AccountManagement.Api;
using Fracture.DialogManagement.Api;
using Fracture.NonPlayerCharacter.Api;
using Fracture.Server.Components;
using Fracture.Server;
using Fracture.Shared.External;
using Fracture.Shared.External.Providers.Ai;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AiBackendConfig>(builder.Configuration.GetSection("AiEndpoint"));

RegisterTypeFromConfiguration(builder, "AiBackendProvider", builder.Services.AddAiProvider);
RegisterTypeFromConfiguration(
    builder,
    "AiPromptTemplateProvider",
    builder.Services.AddPromptTemplateProvider
);

builder.Services
    .AddAccountManagementModule()
    .AddDialogManagementModule(builder.Configuration.GetConnectionString("RedisDialogue")!)
    .AddNonPlayerCharacterModule(
        builder.Configuration.GetConnectionString("NonPlayerCharacterDbContext")!
    );

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.Run();

static void RegisterTypeFromConfiguration(
    WebApplicationBuilder builder,
    string configuration,
    Func<Type, IServiceCollection> action
)
{
    string? implementationType = builder.Configuration[configuration];
    if (implementationType == null)
        throw new InvalidOperationException($"Invalid configuration, {configuration} is null");

    Type? aiBackendType = Type.GetType(implementationType);

    if (aiBackendType != null)
    {
        action.Invoke(aiBackendType);
    }
    else
    {
        throw new InvalidOperationException($"Invalid configuration for {configuration}");
    }
}
