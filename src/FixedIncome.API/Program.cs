using Carter;
using FixedIncome.API.Extensions;
using FixedIncome.API.Middlewares;
using FixedIncome.Application.Configuration;
using FixedIncome.Infrastructure.BackgroundJobs;
using FixedIncome.Infrastructure.BackgroundJobs.Abstractions;
using FixedIncome.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

services.AddApplicationDependencyInjection();

services.AddRepositories();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCarter();

services.AddConfigurations(builder.Configuration);
services.AddServices();
services.AddDbServices();

services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
services.AddHostedService<BackgroundOutboxJob>();
services.AddHostedService<BackgroundEmailJob>();

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

app.UseMiddleware<EndpointTimeLogger>();
app.MapCarter();
app.Run();

public partial class Program() { }