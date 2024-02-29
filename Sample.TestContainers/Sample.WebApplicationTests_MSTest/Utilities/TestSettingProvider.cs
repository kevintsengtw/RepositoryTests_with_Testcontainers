using Microsoft.Extensions.Configuration;

namespace Sample.WebApplicationTests_MSTest.Utilities;

/// <summary>
/// Class TestSettingProvider
/// </summary>
public static class TestSettingProvider
{
    public static string SettingFile { get; set; } = "TestSettings.json";

    /// <summary>
    /// 取得建立測試資料庫指定使用的 docker 設定資料
    /// </summary>
    /// <returns>System.String.</returns>
    public static Mssql GetDatabaseSettings()
    {
        var configuration = GetConfiguration();

        var databaseSettings = new Mssql
        {
            Image = configuration["Mssql:Image"],
            Tag = configuration["Mssql:Tag"],
            SaPassword = configuration["Mssql:SaPassword"],
            ContainerName = configuration["Mssql:ContainerName"],
            ContainerReadyMessage = configuration["Mssql:ContainerReadyMessage"],
            EnvironmentSettings = GetEnvironmentSettings(configuration, "Mssql:EnvironmentSettings"),
            HostPort = string.IsNullOrWhiteSpace(configuration["Mssql:HostPort"])
                ? GetRandomPort()
                : configuration["Mssql:HostPort"].Equals("0")
                    ? GetRandomPort()
                    : ushort.TryParse(configuration["Mssql:HostPort"], out var databasePort)
                        ? databasePort
                        : GetRandomPort(),
            ContainerPort = string.IsNullOrWhiteSpace(configuration["Mssql:ContainerPort"])
                ? (ushort)1433
                : ushort.TryParse(configuration["Mssql:ContainerPort"], out var containerPort)
                    ? containerPort
                    : (ushort)1433
        };

        return databaseSettings;
    }

    /// <summary>
    /// Get Environment Name
    /// </summary>
    /// <returns></returns>
    public static string GetEnvironmentName(Type typeOfTarget)
    {
        var configuration = GetConfiguration();

        var environmentName = string.IsNullOrWhiteSpace(configuration["EnvironmentName"])
            ? typeOfTarget.Assembly.GetName().Name?.ToLower().Replace(".", "-")
            : configuration["EnvironmentName"].ToLower();

        return environmentName;
    }

    /// <summary>
    /// Get Environment Variables Settings
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="section"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetEnvironmentSettings(IConfigurationRoot configuration, string section)
    {
        var environments = new Dictionary<string, string>();

        var children = configuration.GetSection(section).GetChildren().ToArray();

        foreach (var item in children.Select(child => child.Value))
        {
            var value = item?.Split("=");
            environments.Add(value![0], value[1]);
        }

        return environments;
    }

    /// <summary>
    /// Get Random Port
    /// </summary>
    /// <returns></returns>
    public static ushort GetRandomPort()
    {
        var rnd = new Random();
        var result = rnd.Next(49152, 65535);
        return (ushort)result;
    }

    /// <summary>
    /// Get ConfigurationRoot
    /// </summary>
    /// <returns></returns>
    private static IConfigurationRoot GetConfiguration()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile(SettingFile).Build();
        return configuration;
    }
}