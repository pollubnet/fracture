using Game.AccountManagement.Api;
using Game.DialogManagement.Api;
using Game.NonPlayerCharacter.Api;
using Game.Shared.External;
using Game.Shared.External.Providers.Ai;
using Game.Shared.External.Providers.Ai.Llama;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AiEndpointConfig>(builder.Configuration.GetSection("AiEndpoint"));
builder.Services.AddAiProvider<LlamaAiProvider>();

builder.Services
    .AddAccountManagementModule()
    .AddDialogManagementModule()
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
