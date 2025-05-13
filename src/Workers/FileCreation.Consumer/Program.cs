using FileCreation.Consumer;
using Microsoft.Extensions.Hosting;
using FileCreation.Consumer.Extensions;
using FixedIncome.Application.FixedIncomeSimulation.Queries.GetFixedBalance;
using Microsoft.Extensions.Configuration;
using FixedIncome.CrossCutting.Extensions;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

var services = builder.Services;


services.AddConfigurations(builder.Configuration);
services.AddMessaging();
services.AddDbServices();
services.AddBasicMediator(typeof(GetFixedBalanceQueryHandler).Assembly);
services.AddServices();
services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();