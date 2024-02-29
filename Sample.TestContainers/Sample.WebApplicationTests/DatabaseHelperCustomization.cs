using System;
using AutoFixture;
using Microsoft.Extensions.Options;
using Sample.WebApplication.Infrastructure.Helpers;
using Sample.WebApplication.Infrastructure.Settings;

namespace Sample.WebApplicationTests;

/// <summary>
/// class DatabaseHelperCustomization
/// </summary>
public class DatabaseHelperCustomization : ICustomization
{
    private static string ConnectionString => ProjectFixture.SampleDbConnectionString;

    public void Customize(IFixture fixture)
    {
        fixture.Register(() => this.DatabaseHelper);
    }

    private DatabaseHelper _databaseHelper;

    private DatabaseHelper DatabaseHelper
    {
        get
        {
            if (this._databaseHelper is not null)
            {
                return this._databaseHelper;
            }

            var databaseConnectionOptions = new DatabaseConnectionOptions { ConnectionString = ConnectionString };
            var options = Options.Create(databaseConnectionOptions);
            this._databaseHelper = new DatabaseHelper(options);
            return this._databaseHelper;
        }
    }
}