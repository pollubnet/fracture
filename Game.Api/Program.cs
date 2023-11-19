using Game.AccountManagement.Api;
using Game.DialogManagement.Api;
using Game.NonPlayerCharacter.Api;
using Game.Shared.External;
using Game.Shared.External.Providers.Ai;

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
}

app.UseAuthorization();

app.MapControllers();

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
