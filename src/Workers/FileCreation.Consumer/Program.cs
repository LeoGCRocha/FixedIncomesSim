using FileCreation.Consumer;
using FileCreation.Consumer.Extensions;
using Microsoft.Extensions.Hosting;
using FixedIncome.CrossCutting.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

var services = builder.Services;

services.AddConfigurations(builder.Configuration);
services.AddMessaging();
services.AddServices();
services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();