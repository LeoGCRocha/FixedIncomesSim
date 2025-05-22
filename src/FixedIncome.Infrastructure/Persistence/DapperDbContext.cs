using Dapper;
using FixedIncome.Infrastructure.Configuration;
using FixedIncome.Infrastructure.Persistence.Abstractions;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.Extensions.Options;
using Npgsql;

namespace FixedIncome.Infrastructure.Persistence;

public class DapperDbContext : IDapperDbContext
{
    private readonly string _connectionString;
    private readonly ILogger<DapperDbContext> _logger;

    public DapperDbContext(PostgresConfiguration configuration, ILogger<DapperDbContext> logger)
    {
        _connectionString = configuration.GetConnectionString();
        _logger = logger;
    }

    private IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }

    public async Task<TResponse?> GetFirstAsync<TResponse>(string query, object? parameters = null)
    {
        using var connection = CreateConnection();
        
        var response = await connection.QueryFirstOrDefaultAsync<TResponse>(query, parameters);
        return response;
    }

    public async Task<IEnumerable<TListResponse>> GetAsync<TListResponse>(string query, object? parameters = null)
    {
        using var connection = CreateConnection();

        var response = await connection.QueryAsync<TListResponse>(query, parameters);
        return response;
    }
}