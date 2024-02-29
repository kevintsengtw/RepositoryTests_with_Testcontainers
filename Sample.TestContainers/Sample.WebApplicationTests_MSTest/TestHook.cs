using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Sample.WebApplicationTests_MSTest.Utilities;
using Sample.WebApplicationTests_MSTest.Utilities.Database;

namespace Sample.WebApplicationTests_MSTest;

[TestClass]
public class TestHook
{
    private static IContainer _databaseContainer;

    private static string DatabaseIp { get; set; }

    private static string DatabaseSaPassword { get; set; }

    private static string DatabaseName => "Sample";

    internal static string SampleDbConnectionString => string.Format(TestDbConnection.Container.Database, DatabaseIp, DatabaseName, DatabaseSaPassword);

    [AssemblyInitialize]
    [Timeout(2000)]
    public static void AssemblyInitialize(TestContext context)
    {
        // Get the Database Setting
        var databaseSetting = TestSettingProvider.GetDatabaseSettings();
        DatabaseIp = $"127.0.0.1,{databaseSetting.HostPort}";
        DatabaseSaPassword = databaseSetting.SaPassword;

        // Create Database Container
        _databaseContainer = TestContainersHelper.CreateDatabaseContainer(databaseSetting, typeof(TestHook));

        // 確認測試用資料庫已經準備好
        var masterConnectionString = string.Format(TestDbConnection.Container.Master, DatabaseIp, DatabaseSaPassword);
        DatabaseCommand.PrintMssqlVersion(masterConnectionString);

        // 在 container 裡的 SQL Server 建立測試用 Database
        DatabaseCommand.CreateDatabase(masterConnectionString, DatabaseName);

        // FluentAssertions 設定: 日期時間使用接近比對的方式，而非完全一致的比對
        SetupDateTimeAssertions();
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        _databaseContainer.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    /// <summary>
    /// FluentAssertions - Setup DateTime AssertionOptions
    /// </summary>
    private static void SetupDateTimeAssertions()
    {
        // FluentAssertions 設定: 日期時間使用接近比對的方式，而非完全一致的比對
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