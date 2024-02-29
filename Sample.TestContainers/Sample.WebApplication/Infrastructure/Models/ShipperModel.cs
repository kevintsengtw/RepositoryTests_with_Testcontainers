using System.ComponentModel.DataAnnotations;

namespace Sample.WebApplication.Infrastructure.Models;

/// <summary>
/// class ShipperModel
/// </summary>
public class ShipperModel
{
    /// <summary>
    /// ShipperId
    /// </summary>
    public int ShipperId { get; set; }

    /// <summary>
    /// CompanyName
    /// </summary>
    [StringLength(maximumLength: 40)]
    public string CompanyName { get; set; }

    /// <summary>
    /// Phone
    /// </summary>
    [StringLength(maximumLength: 24)]
    public string Phone { get; set; }
}