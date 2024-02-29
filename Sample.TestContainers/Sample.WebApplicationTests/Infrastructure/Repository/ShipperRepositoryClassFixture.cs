using Sample.WebApplication.Infrastructure.Models;
using Sample.WebApplication.Infrastructure.Repository;
using Sample.WebApplicationTests.TestData;
using Sample.WebApplicationTests.Utilities.Database;

namespace Sample.WebApplicationTests.Infrastructure.Repository;

/// <summary>
/// class ShipperRepositoryClassFixture
/// </summary>
public class ShipperRepositoryClassFixture : RepositoryFixture, IDisposable
{
    public readonly ShipperRepository SystemUnderTest;
    
    public ShipperRepositoryClassFixture()
    {
        this.SystemUnderTest = new ShipperRepository(DatabaseHelper);
        this.CreateTable();
    }

    public void Dispose()
    {
        this.DropTable();
    }

    private void CreateTable()
    {
        TableCommands.Drop(ProjectFixture.SampleDbConnectionString, TableNames.Shippers);

        var filePath = Path.Combine("TestData", "TableSchemas", "Sample_Shippers_Create.sql");
        var script = File.ReadAllText(filePath);
        TableCommands.Create(ProjectFixture.SampleDbConnectionString, script);
    }

    private void DropTable()
    {
        TableCommands.Drop(ProjectFixture.SampleDbConnectionString, TableNames.Shippers);
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