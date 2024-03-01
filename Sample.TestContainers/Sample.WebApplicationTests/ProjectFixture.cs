using System.IO.Abstractions;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Sample.WebApplicationTests.Utilities;
using Sample.WebApplicationTests.Utilities.Database;

namespace Sample.WebApplicationTests;

/// <summary>
/// Class ProjectCollectionFixture
/// </summary>
[CollectionDefinition(nameof(ProjectCollectionFixture))]
public class ProjectCollectionFixture : ICollectionFixture<ProjectFixture>;

/// <summary>
/// Class ProjectFixture
/// </summary>
public class ProjectFixture : IAsyncLifetime
{
    private static IFileSystem FileSystem => new FileSystem();

    private static string SettingFile => FileSystem.Path.Combine("Settings", "TestSettings.json");

    private static string DatabaseIp { get; set; }

    private static string DatabaseSaPassword { get; set; }

    private static string DatabaseName => "Sample";

    private static IContainer _mssqlContainer;

    private static string MasterConnectionString =>
        string.Format(TestDbConnection.Container.Master, DatabaseIp, DatabaseSaPassword);

    internal static string SampleDbConnectionString =>
        string.Format(TestDbConnection.Container.Database, DatabaseIp, DatabaseName, DatabaseSaPassword);

    public ProjectFixture()
    {
        //-- Create Mssql Server
        TestSettingProvider.SettingFile = SettingFile;
        var databaseSettings = TestSettingProvider.GetDatabaseSettings();
        DatabaseIp = $"127.0.0.1,{databaseSettings.HostPort}";
        DatabaseSaPassword = databaseSettings.SaPassword;
        _mssqlContainer = MssqlFixture.CreateContainer(databaseSettings, typeof(ProjectFixture));
    }

    public async Task InitializeAsync()
    {
        await _mssqlContainer.StartAsync().ConfigureAwait(false);

        DatabaseCommand.PrintMssqlVersion(MasterConnectionString);

        // 在 container 裡的 SQL Server 建立測試用 Database
        DatabaseHelper.CreateDatabase(MasterConnectionString, DatabaseName);

        //-- Create Tables & Insert Data to Database
        DatabaseHelper.CreateTables(SampleDbConnectionString);

        SetupDateTimeAssertions();
    }

    public Task DisposeAsync()
    {
        return _mssqlContainer.StopAsync();
    }

    //---------------------------------------------------------------------------------------------

    /// <summary>
    /// FluentAssertions - Setup DateTime AssertionOptions
    /// </summary>
    private static void SetupDateTimeAssertions()
    {
        // FluentAssertions 設定 : 日期時間使用接近比對的方式，而非完全一致的比對
        AssertionOptions.AssertEquivalencyUsing(options =>
        {
            options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(1000)))
                   .WhenTypeIs<DateTime>();

            options.Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(1000)))
                   .WhenTypeIs<DateTimeOffset>();

            return options;
        });
    }
}