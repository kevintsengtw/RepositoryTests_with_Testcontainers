using System.Text;
using Dapper;

namespace Sample.WebApplicationTests.Utilities.Database;

/// <summary>
/// Class TableCommands.
/// </summary>
public static class TableCommands
{
    /// <summary>
    /// Drops the table.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="tableName">Name of the table.</param>
    /// <returns>System.String.</returns>
    /// <exception cref="System.ArgumentNullException">tableName - please input tableName.</exception>
    /// <exception cref="ArgumentNullException">please input tableName.</exception>
    public static void Drop(string connectionString, string tableName)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "please input connectionString.");
        }

        if (string.IsNullOrWhiteSpace(tableName))
        {
            throw new ArgumentNullException(nameof(tableName), "please input tableName.");
        }

        using var conn = TestDbConnection.GetSqlConnection(connectionString);
        conn.Open();

        var sqlCommand = new StringBuilder();
        sqlCommand.AppendLine($@"IF OBJECT_ID('dbo.{tableName}', 'U') IS NOT NULL");
        sqlCommand.AppendLine($@"  DROP TABLE dbo.{tableName}; ");

        conn.Execute($"{sqlCommand}");
    }

    /// <summary>
    /// Truncates the table.
    /// </summary>
    /// <param name="connectionString">connectionString</param>
    /// <param name="tableName">Name of the table.</param>
    /// <returns>System.String.</returns>
    /// <exception cref="ArgumentNullException">please input tableName.</exception>
    public static void Truncate(string connectionString, string tableName)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "please input connectionString.");
        }

        if (string.IsNullOrWhiteSpace(tableName))
        {
            throw new ArgumentNullException(nameof(tableName), "please input tableName.");
        }

        using var conn = TestDbConnection.GetSqlConnection(connectionString);
        conn.Open();

        var sqlCommand = new StringBuilder();
        sqlCommand.AppendLine($@"IF OBJECT_ID('dbo.{tableName}', 'U') IS NOT NULL");
        sqlCommand.AppendLine($@"  TRUNCATE TABLE dbo.{tableName}; ");

        conn.Execute($"{sqlCommand}");
    }

    /// <summary>
    /// Creates the table.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="sqlCommand">The SQL command.</param>
    /// <exception cref="System.ArgumentNullException">
    /// connectionString - please input connectionString.
    /// or
    /// sqlCommand - please input sqlCommand.
    /// </exception>
    public static void Create(string connectionString, string sqlCommand)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "please input connectionString.");
        }

        if (string.IsNullOrWhiteSpace(sqlCommand))
        {
            throw new ArgumentNullException(nameof(sqlCommand), "please input sqlCommand.");
        }

        using (var conn = TestDbConnection.GetSqlConnection(connectionString))
        {
            conn.Open();
            conn.Execute(sqlCommand);
        }
    }
}