namespace Sample.WebApplicationTests.TestData;

public static class TableNames
{
    public static string Shippers => "Shippers";

    public static IEnumerable<string> TableNameCollection => new List<string>
    {
        Shippers
    };
}