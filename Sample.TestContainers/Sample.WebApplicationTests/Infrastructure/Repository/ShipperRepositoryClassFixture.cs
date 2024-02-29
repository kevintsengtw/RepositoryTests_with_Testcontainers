using Sample.WebApplication.Infrastructure.Models;
using Sample.WebApplicationTests.TestData;
using Sample.WebApplicationTests.Utilities.Database;

namespace Sample.WebApplicationTests.Infrastructure.Repository;

/// <summary>
/// class ShipperRepositoryClassFixture
/// </summary>
public class ShipperRepositoryClassFixture : RepositoryFixture, IDisposable
{
    public ShipperRepositoryClassFixture()
    {
        this.CreateTable();
    }

    public void Dispose()
    {
        this.DropTable();
    }

    private void CreateTable()
    {
        TableCommands.Drop(this.SampleConnectionString, TableNames.Shippers);

        var filePath = Path.Combine("TestData", "TableSchemas", "Sample_Shippers_Create.sql");
        var script = File.ReadAllText(filePath);
        TableCommands.Create(this.SampleConnectionString, script);
    }

    private void DropTable()
    {
        TableCommands.Drop(this.SampleConnectionString, TableNames.Shippers);
    }

    public static void InsertData(ShipperModel model)
    {
        const string sqlCommand =
            """
            SET IDENTITY_INSERT dbo.Shippers ON
            Insert into Shippers (ShipperID, CompanyName, Phone)
            Values (@ShipperID, @CompanyName, @Phone);
            SET IDENTITY_INSERT dbo.Shippers OFF
            """;

        DatabaseCommand.ExecuteSqlCommand(ProjectFixture.SampleDbConnectionString, sqlCommand, model);
    }
}