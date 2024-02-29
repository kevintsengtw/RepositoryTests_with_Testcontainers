using System;
using System.Data;

namespace Sample.WebApplication.Infrastructure.Helpers;

/// <summary>
/// interface IDatabaseHelper
/// </summary>
public interface IDatabaseHelper
{
    /// <summary>
    /// Gets the connection.
    /// </summary>
    /// <returns></returns>
    IDbConnection GetConnection();
}