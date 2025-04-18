using System.Data;

namespace FixedIncome.Infrastructure.Persistence.Abstractions;

public interface IDapperDbContext
{
    public void Connection();
    public Task<TResponse?> ExecuteAsync<TResponse>(string query, object? parameters);
}