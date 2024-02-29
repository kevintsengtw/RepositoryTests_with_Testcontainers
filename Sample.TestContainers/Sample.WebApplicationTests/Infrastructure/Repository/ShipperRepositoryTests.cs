using AutoFixture;
using FluentAssertions;
using Sample.WebApplication.Infrastructure.Models;
using Sample.WebApplication.Infrastructure.Repository;
using Sample.WebApplicationTests.TestData;
using Sample.WebApplicationTests.Utilities.Database;

namespace Sample.WebApplicationTests.Infrastructure.Repository;

[Collection(nameof(ProjectCollectionFixture))]
public sealed class ShipperRepositoryTests : IClassFixture<ShipperRepositoryClassFixture>, IDisposable
{
    private readonly ShipperRepositoryClassFixture _classFixture;

    private readonly ShipperRepository _systemUnderTest;

    public ShipperRepositoryTests(ShipperRepositoryClassFixture classFixture)
    {
        this._classFixture = classFixture;

        this._systemUnderTest = new ShipperRepository(this._classFixture.DatabaseHelper);
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            TableCommands.Truncate(this._classFixture.SampleConnectionString, TableNames.Shippers);    
        }
    }

    //---------------------------------------------------------------------------------------------
    // IsExistsAsync

    [Fact]
    public async Task IsExistsAsync_輸入的ShipperId為0時_應拋出ArgumentOutOfRangeException()
    {
        // arrange
        var shipperId = 0;

        // act
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => this._systemUnderTest.IsExistsAsync(shipperId));

        // assert
        exception.Message.Should().Contain(nameof(shipperId));
    }

    [Fact]
    public async Task IsExistsAsync_輸入的ShipperId為負1時_應拋出ArgumentOutOfRangeException()
    {
        // arrange
        var shipperId = -1;

        // act
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => this._systemUnderTest.IsExistsAsync(shipperId));

        // assert
        exception.Message.Should().Contain(nameof(shipperId));
    }

    [Fact]
    public async Task IsExistsAsync_輸入的ShipperId_資料不存在_應回傳false()
    {
        // arrange
        var shipperId = 99;

        // act
        var actual = await this._systemUnderTest.IsExistsAsync(shipperId);

        // assert
        actual.Should().BeFalse();
    }

    [Fact]
    public async Task IsExistsAsync_輸入的ShipperId_資料有存在_應回傳True()
    {
        // arrange
        var model = this._classFixture.Fixture.Build<ShipperModel>().Create();
        var shipperId = model.ShipperId;
        ShipperRepositoryClassFixture.InsertData(model);

        // act
        var actual = await this._systemUnderTest.IsExistsAsync(shipperId);

        // assert
        actual.Should().BeTrue();
    }

    //---------------------------------------------------------------------------------------------
    // GetAsync

    [Fact]
    public async Task GetAsync_輸入的ShipperId為0時_應拋出ArgumentOutOfRangeException()
    {
        // arrange
        var shipperId = 0;

        // act
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => this._systemUnderTest.GetAsync(shipperId));

        // assert
        exception.Message.Should().Contain(nameof(shipperId));
    }

    [Fact]
    public async Task GetAsync_輸入的ShipperId為負1時_應拋出ArgumentOutOfRangeException()
    {
        // arrange
        var shipperId = -1;

        // act
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => this._systemUnderTest.GetAsync(shipperId));

        // assert
        exception.Message.Should().Contain(nameof(shipperId));
    }

    [Fact]
    public async Task GetAsync_輸入的ShipperId_資料不存在_應回傳null()
    {
        // arrange
        var shipperId = 99;

        // act
        var actual = await this._systemUnderTest.GetAsync(shipperId);

        // assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task GetAsync_輸入的ShipperId_資料有存在_應回傳model()
    {
        // arrange
        var model = this._classFixture.Fixture.Build<ShipperModel>().Create();
        var shipperId = model.ShipperId;
        ShipperRepositoryClassFixture.InsertData(model);

        // act
        var actual = await this._systemUnderTest.GetAsync(shipperId);

        // assert
        actual.Should().NotBeNull();
        actual.ShipperId.Should().Be(shipperId);
    }
    
    //---------------------------------------------------------------------------------------------
    // GetTotalCountAsync

    [Fact]
    public async Task GetTotalCountAsync_資料表裡無資料_應回傳0()
    {
        // arrange
        var expected = 0;
        
        // act
        var actual = await this._systemUnderTest.GetTotalCountAsync();
        
        // assert
        actual.Should().Be(expected);
    }
    
    [Fact]
    public async Task GetTotalCountAsync_資料表裡有10筆資料_應回傳10()
    {
        // arrange
        var expected = 10;

        var models = this._classFixture.Fixture.CreateMany<ShipperModel>(10);

        foreach (var model in models)
        {
            ShipperRepositoryClassFixture.InsertData(model);
        }
        
        // act
        var actual = await this._systemUnderTest.GetTotalCountAsync();
        
        // assert
        actual.Should().Be(expected);
    }
    
    //---------------------------------------------------------------------------------------------
    // GetAllAsync

    [Fact]
    public async Task GetAllAsync_資料表裡無資料_應回傳空集合()
    {
        // arrange
        
        // act
        var actual = await this._systemUnderTest.GetAllAsync();
        
        // assert
        actual.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAllAsync_資料表裡有10筆資料_回傳的集合裡有10筆()
    {
        // arrange
        var models = this._classFixture.Fixture.CreateMany<ShipperModel>(10);

        foreach (var model in models)
        {
            ShipperRepositoryClassFixture.InsertData(model);
        }
        
        // act
        var actual = await this._systemUnderTest.GetAllAsync();
        
        // assert
        actual.Should().NotBeEmpty();
        actual.Should().HaveCount(10);
    }    
    
    //---------------------------------------------------------------------------------------------
    // CreateAsync

    [Fact]
    public async Task CreateAsync_輸入的model為null時_應拋出ArgumentNullException()
    {
        // arrange
        ShipperModel model = null;

        // act
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => this._systemUnderTest.CreateAsync(model));

        // assert
        exception.Message.Should().Contain(nameof(model));
    }

    [Fact]
    public async Task CreateAsync_輸入一個有資料的model_新增完成_回傳Result的Success應為true()
    {
        // arrange
        var model = this._classFixture.Fixture.Build<ShipperModel>().Create();

        // act
        var actual = await this._systemUnderTest.CreateAsync(model);

        // assert
        actual.Success.Should().BeTrue();
        actual.AffectRows.Should().Be(1);
    }
    
    //---------------------------------------------------------------------------------------------
    // UpdateAsync

    [Fact]
    public async Task UpdateAsync_輸入的model為null時_應拋出ArgumentNullException()
    {
        // arrange
        ShipperModel model = null;

        // act
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => this._systemUnderTest.UpdateAsync(model));

        // assert
        exception.Message.Should().Contain(nameof(model));
    }
    
    
    [Fact]
    public async Task UpdateAsync_輸入model_要修改的資料並不存在_更新錯誤_回傳Result的Success應為false()
    {
        // arrange
        var model = this._classFixture.Fixture.Build<ShipperModel>().Create();
        
        // act
        var actual = await this._systemUnderTest.UpdateAsync(model);
        
        // assert
        actual.Success.Should().BeFalse();
        actual.Message.Should().Be("資料更新錯誤");
    }
    
    [Fact]
    public async Task UpdateAsync_輸入model_要修改的資料存在_更新完成_回傳Result的Success應為true()
    {
        // arrange
        var model = this._classFixture.Fixture.Build<ShipperModel>().Create();
        
        ShipperRepositoryClassFixture.InsertData(model);

        model.CompanyName = "update";
        
        // act
        var actual = await this._systemUnderTest.UpdateAsync(model);
        
        // assert
        actual.Success.Should().BeTrue();
    }
    
    //---------------------------------------------------------------------------------------------
    // DeleteAsync
    
    [Fact]
    public async Task DeleteAsync_輸入的ShipperId為0時_應拋出ArgumentOutOfRangeException()
    {
        // arrange
        var shipperId = 0;

        // act
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => this._systemUnderTest.DeleteAsync(shipperId));

        // assert
        exception.Message.Should().Contain(nameof(shipperId));
    }

    [Fact]
    public async Task DeleteAsync_輸入的ShipperId為負1時_應拋出ArgumentOutOfRangeException()
    {
        // arrange
        var shipperId = -1;

        // act
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => this._systemUnderTest.DeleteAsync(shipperId));

        // assert
        exception.Message.Should().Contain(nameof(shipperId));
    }
    
    [Fact]
    public async Task DeleteAsync_輸入ShipperId_要刪除的資料並不存在_刪除錯誤_回傳Result的Success應為false()
    {
        // arrange
        var shipperId = 999;
        
        // act
        var actual = await this._systemUnderTest.DeleteAsync(shipperId);
        
        // assert
        actual.Success.Should().BeFalse();
        actual.Message.Should().Be("資料刪除錯誤");
    } 
    
    [Fact]
    public async Task DeleteAsync_輸入model_要刪除的資料存在_刪除完成_回傳Result的Success應為true()
    {
        // arrange
        var model = this._classFixture.Fixture.Create<ShipperModel>();
        
        ShipperRepositoryClassFixture.InsertData(model);
        
        // act
        var actual = await this._systemUnderTest.DeleteAsync(model.ShipperId);
        
        // assert
        actual.Success.Should().BeTrue();
    } 
}