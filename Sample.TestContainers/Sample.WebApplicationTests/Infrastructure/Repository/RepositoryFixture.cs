using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Microsoft.Extensions.Options;
using Sample.WebApplication.Infrastructure.Helpers;
using Sample.WebApplication.Infrastructure.Settings;
using DatabaseHelper = Sample.WebApplication.Infrastructure.Helpers.DatabaseHelper;

namespace Sample.WebApplicationTests.Infrastructure.Repository;

/// <summary>
/// Class RepositoryFixture
/// </summary>
public class RepositoryFixture
{
    internal string SampleConnectionString { get; set; }

    internal IFixture Fixture { get; set; }

    internal IDatabaseHelper DatabaseHelper { get; set; }

    public RepositoryFixture()
    {
        this.SampleConnectionString = ProjectFixture.SampleDbConnectionString;

        var options = Options.Create(new DatabaseConnectionOptions { ConnectionString = this.SampleConnectionString });

        this.DatabaseHelper = new DatabaseHelper(options);

        this.Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
    }
}