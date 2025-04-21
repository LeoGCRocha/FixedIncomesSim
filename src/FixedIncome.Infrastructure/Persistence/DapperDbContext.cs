using Dapper;
using FixedIncome.Infrastructure.Configuration;
using FixedIncome.Infrastructure.Persistence.Abstractions;
using Microsoft.Extensions.Logging;
using System.Data;
using Npgsql;

namespace FixedIncome.Infrastructure.Persistence;

public class DapperDbContext : IDisposable, IDapperDbContext
{
    private IDbConnection? _connection;
    private readonly string _connectionString;
    private readonly ILogger<DapperDbContext> _logger;

    public DapperDbContext(IBaseConfiguration configuration, ILogger<DapperDbContext> logger)
    {
        _connectionString = configuration.GetConnectionString();
        _logger = logger;
    }

    public void Connection()
    {
        if (_connection is { State: ConnectionState.Open }) return;
        
        try
        {
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cannot open the connection on Dapper {Error}", ex.Message);
            throw;
        }
    }

    public async Task<TResponse?> GetFirstAsync<TResponse>(string query, object? parameters = null)
    {
        if (_connection is null)
            Connection();
        
        var response = await _connection!.QueryFirstOrDefaultAsync<TResponse>(query, parameters);
        return response;
    }

    public async Task<IEnumerable<TListResponse>> GetAsync<TListResponse>(string query, object? parameters = null)
    {
        if (_connection is null)
            Connection();

        var response = await _connection!.QueryAsync<TListResponse>(query, parameters);
        return response;
    }

    public void Dispose()
    {
        if (_connection is { State: ConnectionState.Open })
            _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}