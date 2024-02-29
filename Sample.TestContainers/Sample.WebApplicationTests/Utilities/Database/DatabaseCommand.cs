using System.IO.Abstractions;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Sample.WebApplicationTests.Utilities.Database;

/// <summary>
/// Class DatabaseCommand
/// </summary>
public static class DatabaseCommand
{
    private static IFileSystem FileSystem => new FileSystem();

    /// <summary>
    /// 建立 Database.
    /// </summary>
    /// <param name="connectionString">The connectionString.</param>
    /// <param name="database">The database.</param>
    public static void CreateDatabase(string connectionString, string database)
    {
        var exists = DatabaseExists(connectionString, database);
        if (exists)
        {
            return;
        }

        using var conn = new SqlConnection(connectionString);
        conn.Open();
        var sqlCommand = $"CREATE DATABASE [{database}];";
        conn.Execute(sqlCommand);
    }

    /// <summary>
    /// 檢查指定的 Database 是否存在.
    /// </summary>
    /// <param name="connectionString">The connectionString.</param>
    /// <param name="database">The database.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    private static bool DatabaseExists(string connectionString, string database)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        var sqlCommand = new StringBuilder();
        sqlCommand.AppendLine($"if exists(select * from sys.databases where name = '{database}')");
        sqlCommand.AppendLine("select 'true'");
        sqlCommand.AppendLine("else ");
        sqlCommand.AppendLine("select 'false'");

        var result = conn.QueryFirstOrDefault<string>($"{sqlCommand}");
        return result!.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Execute Db Script
    /// </summary>
    /// <param name="connectionString">connectionString</param>
    /// <param name="filePath">filePath</param>
    public static void ExecuteDbScript(string connectionString, string filePath)
    {
        if (FileSystem.File.Exists(filePath) is false)
        {
            return;
        }

        using var conn = new SqlConnection(connectionString);
        conn.Open();

        using var trans = conn.BeginTransaction();
        var script = FileSystem.File.ReadAllText(filePath);
        conn.Execute(sql: script, transaction: trans);
        trans.Commit();
    }

    /// <summary>
    /// Executes the SQL command.
    /// </summary>
    /// <param name="connectionString">The connectionString.</param>
    /// <param name="commandText">The command text.</param>
    /// <param name="param">param</param>
    public static void ExecuteSqlCommand(string connectionString, string commandText, object? param = null)
    {
        using var connection = new SqlConnection(connectionString);
        if (param is null)
        {
            connection.Execute(commandText);
        }
        else
        {
            connection.Execute(commandText, param);
        }
    }

    /// <summary>
    /// Print Mssql Version.
    /// </summary>
    /// <param name="connectionString">connectionString</param>
    public static void PrintMssqlVersion(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand("SELECT @@VERSION", connection);
        connection.Open();

        var reader = command.ExecuteReader();
        reader.Read();

        Console.WriteLine($"MSSQL Version: {reader.GetString(0)}");
    }
}