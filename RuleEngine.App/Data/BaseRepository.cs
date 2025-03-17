using System.Data;
using Npgsql;

namespace RuleEngine.App;

public abstract class BaseRepository
{
    private readonly string _connectionString;

    protected BaseRepository(IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("ConnectionString");
        _connectionString = connectionString.Value;
    }

    protected virtual async Task<IDbConnection> GetDbConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}