using FixedIncome.API.Extensions;
using FixedIncome.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddDbServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();