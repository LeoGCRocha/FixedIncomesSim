using Carter;
using FixedIncome.API.Extensions;
using FixedIncome.Application.Configuration;
using FixedIncome.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDependencyInjection();
builder.Services.AddCarter();
builder.Services.AddRepositories();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddDbServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Add swagger version
    app.UseSwagger();
    app.UseSwaggerUI();
    
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.MapCarter();
app.Run();