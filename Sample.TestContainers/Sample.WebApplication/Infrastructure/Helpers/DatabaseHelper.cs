using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Sample.WebApplication.Infrastructure.Settings;

namespace Sample.WebApplication.Infrastructure.Helpers;

/// <summary>
/// class DatabaseHelper
/// </summary>
public class DatabaseHelper : IDatabaseHelper
{
    private readonly DatabaseConnectionOptions _databaseConnectionOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseHelper"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public DatabaseHelper(IOptions<DatabaseConnectionOptions> options)
    {
        this._databaseConnectionOptions = options.Value;
    }

    private string ConnectionString => this._databaseConnectionOptions.ConnectionString;
    
    /// <summary>
    /// Gets the connection.
    /// </summary>
    /// <returns></returns>
    public IDbConnection GetConnection()
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(this.ConnectionString)
        {
            TrustServerCertificate = true
        };

        var connection = new SqlConnection(connectionStringBuilder.ConnectionString);
        return connection;
    }
}