using System;

namespace Sample.WebApplication.Controllers.Parameters;

/// <summary>
/// class ShipperUpdateParameter
/// </summary>
public class ShipperUpdateParameter
{
    /// <summary>
    /// ShipperId
    /// </summary>
    public int ShipperId { get; set; }

    /// <summary>
    /// CompanyName
    /// </summary>
    public string CompanyName { get; set; }

    /// <summary>
    /// Phone
    /// </summary>
    public string Phone { get; set; }
}