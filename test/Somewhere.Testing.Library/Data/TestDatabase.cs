using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Somewhere.Data.Abstractions;
using Npgsql;

namespace Somewhere.Testing.Library.Data;

public class TestDatabase : IDatabase
{
    private readonly string _connectionString;
    
    public TestDatabase(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Somewhere") ?? throw new InvalidOperationException();
    }

    public IDbConnection Connect()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public void Initialize()
    {
        DropTopicsTable();
        CreateTopicsTable();
    }
    
    /// <summary>
    /// Drop the topics table from the application database.
    /// </summary>
    private void DropTopicsTable()
    {
        using var connection = Connect();
        const string sql = @"drop table if exists topics;";

        connection.Execute(sql);
    }

    /// <summary>
    /// Create the topics table in the application database.
    /// </summary>
    private void CreateTopicsTable()
    {
        using var connection = Connect();
        const string sql = @"create table topics (
                                id          serial primary key,
                                name        text   not null unique,
                                description text
                             );";

        connection.Execute(sql);
    }
}