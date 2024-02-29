using Microsoft.Data.SqlClient;

namespace Sample.WebApplicationTests_MSTest.Utilities.Database;

public static class TestDbConnection
{
    public static class Container
    {
        public const string Master =
            @"Data Source={0};Initial Catalog=master;Persist Security Info=True;User ID=sa;Password={1};Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;TrustServerCertificate=True";

        public const string Database =
            @"Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID=sa;Password={2};Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;TrustServerCertificate=True";
    }

    /// <summary>
    /// 輸入 ConnectionString 以取得 SqlConnection.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>SqlConnection.</returns>
    public static SqlConnection GetSqlConnection(string connectionString)
    {
        var connection = new SqlConnection(connectionString);
        return connection;
    }
}