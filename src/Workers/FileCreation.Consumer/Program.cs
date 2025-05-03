using FileCreation.Consumer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;

services.AddHostedService<Worker>();

services.AddConfigurations(builder.Configuration);

var host = builder.Build();
await host.RunAsync();