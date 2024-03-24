using FluentValidation;
using MessagingApp.Api;
using MessagingApp.Api.Endpoints;
using MessagingApp.Application;
using MessagingApp.Infrastructure;
using ServiceConfiguration = MessagingApp.Api.ServiceConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

builder.Services.RegisterInfrastructure(builder.Configuration);
builder.Services.RegisterSettings(builder.Configuration);

builder.Services.RegisterHandlers();
builder.Services.ConfigureCors();

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

app.MapEndpoints();

app.Run();