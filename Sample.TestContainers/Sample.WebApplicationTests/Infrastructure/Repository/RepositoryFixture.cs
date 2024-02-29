using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Sample.WebApplication.Infrastructure.Helpers;
using DatabaseHelper = Sample.WebApplication.Infrastructure.Helpers.DatabaseHelper;

namespace Sample.WebApplicationTests.Infrastructure.Repository;

/// <summary>
/// Class RepositoryFixture
/// </summary>
public class RepositoryFixture
{
    internal static IFixture Fixture => new Fixture().Customize(new AutoNSubstituteCustomization())
                                                     .Customize(new DatabaseHelperCustomization());

    internal static IDatabaseHelper DatabaseHelper => Fixture.Create<DatabaseHelper>();
}