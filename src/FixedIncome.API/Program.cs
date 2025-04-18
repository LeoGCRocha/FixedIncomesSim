using Carter;
using FixedIncome.API.Extensions;
using FixedIncome.Application.Configuration;
using FixedIncome.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

services.AddApplicationDependencyInjection();
services.AddRepositories();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCarter();

builder.AddDbServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // TODO Add swagger version
    app.UseSwagger();
    app.UseSwaggerUI();
    
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.MapCarter();
app.Run();