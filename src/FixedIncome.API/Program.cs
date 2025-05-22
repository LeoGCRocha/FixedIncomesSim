using Polly;
using Carter;
using FixedIncome.API.Extensions;
using FixedIncome.API.Middlewares;
using FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalance;
using FixedIncome.CrossCutting.Extensions;
using FixedIncome.Infrastructure.BackgroundJobs;
using FixedIncome.Infrastructure.BackgroundJobs.Abstractions;
using FixedIncome.Infrastructure.CircuitBreaker;
using FixedIncome.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

services.AddRepositories();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddCarter();

services.AddConfigurations(builder.Configuration);

services.AddMessaging();
services.AddServices();
services.AddBasicMediator(typeof(GetFixedBalanceQueryHandler).Assembly);
services.AddDbServices();

services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
services.AddHostedService<BackgroundOutboxJob>();
services.AddHostedService<BackgroundEmailJob>();

services.AddSingleton<ISyncPolicy>(_ => CircuitBreakerFactory.CreateMessagePolicy());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseMiddleware<EndpointTimeLogger>();
app.MapCarter();
app.Run();

public partial class Program() { }