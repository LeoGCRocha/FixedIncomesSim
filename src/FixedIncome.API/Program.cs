using FixedIncome.API.Extensions;
using FixedIncome.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// TODO Continuar daqui 
// TODO Criar docker para o banco de dados

builder.AddDbServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var dbContext = app.Services.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();