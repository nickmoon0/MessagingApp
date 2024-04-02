using FluentValidation;
using MessagingApp.Api;
using MessagingApp.Api.Endpoints;
using MessagingApp.Api.Hubs;
using MessagingApp.Api.Middleware;
using MessagingApp.Application;
using MessagingApp.Infrastructure;
using ServiceConfiguration = MessagingApp.Api.ServiceConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

builder.Services.RegisterInfrastructure(builder.Configuration);
builder.Services.RegisterSettings(builder.Configuration);

builder.Services.RegisterHandlers();
builder.Services.ConfigureCors();

builder.Services.AddScoped<JwtParsingMiddleware>();

builder.ConfigureAuthentication();

var app = builder.Build();

// Inject config into helper class
MessagingApp.Api.Common.Helpers.Configuration = builder.Configuration;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(ServiceConfiguration.LocalhostCorsPolicy);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JwtParsingMiddleware>(); // Register after auth so that all tokens at this point have been validated

app.MapEndpoints();

app.MapHub<ConversationHub>("/hub/message");

app.Run();