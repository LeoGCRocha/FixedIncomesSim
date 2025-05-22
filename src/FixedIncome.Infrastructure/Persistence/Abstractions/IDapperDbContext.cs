namespace FixedIncome.Infrastructure.Persistence.Abstractions;

public interface IDapperDbContext
{
    public Task<TResponse?> GetFirstAsync<TResponse>(string query, object? parameters);
    public Task<IEnumerable<TListResponse>> GetAsync<TListResponse>(string query, object? parameters);
}