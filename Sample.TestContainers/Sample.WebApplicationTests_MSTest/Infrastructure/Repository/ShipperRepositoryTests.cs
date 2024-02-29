using AutoFixture;
using FluentAssertions;
using Sample.WebApplication.Infrastructure.Models;
using Sample.WebApplication.Infrastructure.Repository;
using Sample.WebApplicationTests_MSTest.TestData;
using Sample.WebApplicationTests_MSTest.Utilities.Database;

namespace Sample.WebApplicationTests_MSTest.Infrastructure.Repository;

[TestClass]
public class ShipperRepositoryTests : BaseRepositoryTests<ShipperRepository>
{
    [TestCleanup]
    public void TestCleanup()
    {
        TableCommands.Truncate(this.ConnectionString, TableNames.Shippers);
    }

    #region -- Prepare to Test --

    public TestContext TestContext { get; set; }

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        TableCommands.Drop(TestHook.SampleDbConnectionString, TableNames.Shippers);
        var filePath = Path.Combine("TestData", "TableSchemas", "Sample_Shippers_Create.sql");
        var script = File.ReadAllText(filePath);
        TableCommands.Create(TestHook.SampleDbConnectionString, script);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        TableCommands.Drop(TestHook.SampleDbConnectionString, TableNames.Shippers);
    }

    #endregion

    //-----------------------------------------------------------------------------------------
    // IsExistsAsync

    [TestMethod]
    public async Task IsExistsAsync_輸入的ShipperId為0時_應拋出ArgumentOutOfRangeException()
    {
        // arrange
        var shipperId = 0;

        // act
        var exception = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(
            () => this.Stub.SystemUnderTest.IsExistsAsync(shipperId));

        // assert
        exception.Message.Should().Contain(nameof(shipperId));
    }

    [TestMethod]
    public async Task IsExistsAsync_輸入的ShipperId為負1時_應拋出ArgumentOutOfRangeException()
    {
        // arrange
        var shipperId = -1;

        // act
        var exception = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(
            () => this.Stub.SystemUnderTest.IsExistsAsync(shipperId));

        // assert
        exception.Message.Should().Contain(nameof(shipperId));
    }

    [TestMethod]
    public async Task IsExistsAsync_輸入的ShipperId_資料不存在_應回傳false()
    {
        // arrange
        var shipperId = 99;

        // act
        var actual = await this.Stub.SystemUnderTest.IsExistsAsync(shipperId);

        // assert
        actual.Should().BeFalse();
    }

    [TestMethod]
    public async Task IsExistsAsync_輸入的ShipperId_資料有存在_應回傳True()
    {
        // arrange
        var model = this.Fixture.Build<ShipperModel>().Create();
        var shipperId = model.ShipperId;
        InsertData(model);

        // act
        var actual = await this.Stub.SystemUnderTest.IsExistsAsync(shipperId);

        // assert
        actual.Should().BeTrue();
    }

    //---------------------------------------------------------------------------------------------
    // GetAsync

    [TestMethod]
    public async Task GetAsync_輸入的ShipperId為0時_應拋出ArgumentOutOfRangeException()
    {
        // arrange
        var shipperId = 0;

        // act
        var exception = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(
            () => this.Stub.SystemUnderTest.GetAsync(shipperId));

        // assert
        exception.Message.Should().Contain(nameof(shipperId));
    }

    [TestMethod]
    public async Task GetAsync_輸入的ShipperId為負1時_應拋出ArgumentOutOfRangeException()
    {
        // arrange
        var shipperId = -1;

        // act
        var exception = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(
            () => this.Stub.SystemUnderTest.GetAsync(shipperId));

        // assert
        exception.Message.Should().Contain(nameof(shipperId));
    }

    [TestMethod]
    public async Task GetAsync_輸入的ShipperId_資料不存在_應回傳null()
    {
        // arrange
        var shipperId = 99;

        // act
        var actual = await this.Stub.SystemUnderTest.GetAsync(shipperId);

        // assert
        actual.Should().BeNull();
    }

    [TestMethod]
    public async Task GetAsync_輸入的ShipperId_資料有存在_應回傳model()
    {
        // arrange
        var model = this.Fixture.Build<ShipperModel>().Create();
        var shipperId = model.ShipperId;
        InsertData(model);

        // act
        var actual = await this.Stub.SystemUnderTest.GetAsync(shipperId);

        // assert
        actual.Should().NotBeNull();
        actual.ShipperId.Should().Be(shipperId);
    }

    //---------------------------------------------------------------------------------------------
    // GetTotalCountAsync

    [TestMethod]
    public async Task GetTotalCountAsync_資料表裡無資料_應回傳0()
    {
        // arrange
        var expected = 0;

        // act
        var actual = await this.Stub.SystemUnderTest.GetTotalCountAsync();

        // assert
        actual.Should().Be(expected);
    }

    [TestMethod]
    public async Task GetTotalCountAsync_資料表裡有10筆資料_應回傳10()
    {
        // arrange
        var expected = 10;

        var models = this.Fixture.CreateMany<ShipperModel>(10);

        foreach (var model in models)
        {
            InsertData(model);
        }

        // act
        var actual = await this.Stub.SystemUnderTest.GetTotalCountAsync();

        // assert
        actual.Should().Be(expected);
    }

    //---------------------------------------------------------------------------------------------
    // GetAllAsync

    [TestMethod]
    public async Task GetAllAsync_資料表裡無資料_應回傳空集合()
    {
        // arrange

        // act
        var actual = await this.Stub.SystemUnderTest.GetAllAsync();

        // assert
        actual.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetAllAsync_資料表裡有10筆資料_回傳的集合裡有10筆()
    {
        // arrange
        var models = this.Fixture.CreateMany<ShipperModel>(10);

        foreach (var model in models)
        {
            InsertData(model);
        }

        // act
        var actual = await this.Stub.SystemUnderTest.GetAllAsync();

        // assert
        actual.Should().NotBeEmpty();
        actual.Should().HaveCount(10);
    }

    //---------------------------------------------------------------------------------------------
    // CreateAsync

    [TestMethod]
    public async Task CreateAsync_輸入的model為null時_應拋出ArgumentNullException()
    {
        // arrange
        ShipperModel model = null;

        // act
        var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(
            () => this.Stub.SystemUnderTest.CreateAsync(model));

        // assert
        exception.Message.Should().Contain(nameof(model));
    }

    [TestMethod]
    public async Task CreateAsync_輸入一個有資料的model_新增完成_回傳Result的Success應為true()
    {
        // arrange
        var model = this.Fixture.Build<ShipperModel>().Create();

        // act
        var actual = await this.Stub.SystemUnderTest.CreateAsync(model);

        // assert
        actual.Success.Should().BeTrue();
        actual.AffectRows.Should().Be(1);
    }

    //---------------------------------------------------------------------------------------------
    // UpdateAsync

    [TestMethod]
    public async Task UpdateAsync_輸入的model為null時_應拋出ArgumentNullException()
    {
        // arrange
        ShipperModel model = null;

        // act
        var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(
            () => this.Stub.SystemUnderTest.UpdateAsync(model));

        // assert
        exception.Message.Should().Contain(nameof(model));
    }


    [TestMethod]
    public async Task UpdateAsync_輸入model_要修改的資料並不存在_更新錯誤_回傳Result的Success應為false()
    {
        // arrange
        var model = this.Fixture.Build<ShipperModel>().Create();

        // act
        var actual = await this.Stub.SystemUnderTest.UpdateAsync(model);

        // assert
        actual.Success.Should().BeFalse();
        actual.Message.Should().Be("資料更新錯誤");
    }

    [TestMethod]
    public async Task UpdateAsync_輸入model_要修改的資料存在_更新完成_回傳Result的Success應為true()
    {
        // arrange
        var model = this.Fixture.Build<ShipperModel>().Create();

        InsertData(model);

        model.CompanyName = "update";

        // act
        var actual = await this.Stub.SystemUnderTest.UpdateAsync(model);

        // assert
        actual.Success.Should().BeTrue();
    }

    //---------------------------------------------------------------------------------------------
    // DeleteAsync

    [TestMethod]
    public async Task DeleteAsync_輸入的ShipperId為0時_應拋出ArgumentOutOfRangeException()
    {
        // arrange
        var shipperId = 0;

        // act
        var exception = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(
            () => this.Stub.SystemUnderTest.DeleteAsync(shipperId));

        // assert
        exception.Message.Should().Contain(nameof(shipperId));
    }

    [TestMethod]
    public async Task DeleteAsync_輸入的ShipperId為負1時_應拋出ArgumentOutOfRangeException()
    {
        // arrange
        var shipperId = -1;

        // act
        var exception = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(
            () => this.Stub.SystemUnderTest.DeleteAsync(shipperId));

        // assert
        exception.Message.Should().Contain(nameof(shipperId));
    }

    [TestMethod]
    public async Task DeleteAsync_輸入ShipperId_要刪除的資料並不存在_刪除錯誤_回傳Result的Success應為false()
    {
        // arrange
        var shipperId = 999;

        // act
        var actual = await this.Stub.SystemUnderTest.DeleteAsync(shipperId);

        // assert
        actual.Success.Should().BeFalse();
        actual.Message.Should().Be("資料刪除錯誤");
    }

    [TestMethod]
    public async Task DeleteAsync_輸入model_要刪除的資料存在_刪除完成_回傳Result的Success應為true()
    {
        // arrange
        var model = this.Fixture.Create<ShipperModel>();

        InsertData(model);

        // act
        var actual = await this.Stub.SystemUnderTest.DeleteAsync(model.ShipperId);

        // assert
        actual.Success.Should().BeTrue();
    }


    //-----------------------------------------------------------------------------------------

    private static void InsertData(ShipperModel model)
    {
        const string sqlCommand =
            """
            SET IDENTITY_INSERT dbo.Shippers ON
            Insert into Shippers (ShipperID, CompanyName, Phone)
            Values (@ShipperID, @CompanyName, @Phone);
            SET IDENTITY_INSERT dbo.Shippers OFF
            """;

        DatabaseCommand.ExecuteSqlCommand(TestHook.SampleDbConnectionString, sqlCommand, model);
    }
}