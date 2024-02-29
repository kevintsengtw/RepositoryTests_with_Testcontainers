using System;
using Microsoft.AspNetCore.Mvc;
using Sample.WebApplication.Controllers.Parameters;
using Sample.WebApplication.Infrastructure.Models;
using Sample.WebApplication.Infrastructure.Repository;

namespace Sample.WebApplication.Controllers;

/// <summary>
/// class ShipperController
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
[Route("api/[controller]")]
public class ShipperController : Controller
{
    private readonly IShipperRepository _shipperRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShipperController"/> class.
    /// </summary>
    /// <param name="shipperRepository">The shipper repository.</param>
    public ShipperController(IShipperRepository shipperRepository)
    {
        this._shipperRepository = shipperRepository;
    }

    /// <summary>
    /// 取得所有 Shipper 資料
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var shippers = await this._shipperRepository.GetAllAsync();
        return this.Ok(shippers);
    }

    /// <summary>
    /// 以 ShipperId 取得 Shipper 資料
    /// </summary>
    /// <param name="shipperId">The shipperId.</param>
    /// <returns></returns>
    [HttpGet("{shipperId}")]
    public async Task<IActionResult> GetAsync(int shipperId)
    {
        if (shipperId <= 0)
        {
            return this.BadRequest("shipperId 輸入錯誤，必須大於 0");
        }

        var exists = await this._shipperRepository.IsExistsAsync(shipperId);
        if (exists is false)
        {
            return this.BadRequest($"ShipperId: {shipperId} 所對映的資料不存在");
        }

        var shipper = await this._shipperRepository.GetAsync(shipperId);
        return this.Ok(shipper);
    }

    /// <summary>
    /// 新增 Shipper 資料
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] ShipperUpdateParameter parameter)
    {
        if (parameter.ShipperId <= 0)
        {
            return this.BadRequest("parameter 的 shipperId 輸入錯誤，必須大於 0");
        }

        if (string.IsNullOrWhiteSpace(parameter.CompanyName))
        {
            return this.BadRequest("CompanyName 輸入錯誤，必須輸入值");
        }

        if (string.IsNullOrWhiteSpace(parameter.Phone))
        {
            return this.BadRequest("Phone 輸入錯誤，必須輸入值");
        }

        var exists = await this._shipperRepository.IsExistsAsync(parameter.ShipperId);
        if (exists)
        {
            return this.BadRequest("已經有重複的 ShipperId 資料");
        }

        var model = new ShipperModel
        {
            ShipperId = parameter.ShipperId,
            CompanyName = parameter.CompanyName,
            Phone = parameter.Phone
        };

        var result = await this._shipperRepository.CreateAsync(model);
        if (result.Success is false)
        {
            return this.BadRequest("資料新增錯誤");
        }

        return this.Ok("資料新增完成");
    }

    /// <summary>
    /// 修改 Shipper 資料
    /// </summary>
    /// <param name="shipperId">The ShipperId.</param>
    /// <param name="parameter">The parameter.</param>
    /// <returns></returns>
    [HttpPut("{shipperId}")]
    public async Task<IActionResult> PutAsync(int shipperId, [FromBody] ShipperUpdateParameter parameter)
    {
        if (shipperId <= 0)
        {
            return this.BadRequest("shipperId 輸入錯誤，必須大於 0");
        }

        if (parameter.ShipperId <= 0)
        {
            return this.BadRequest("parameter 的 ShipperId 輸入錯誤，必須大於 0");
        }

        if (string.IsNullOrWhiteSpace(parameter.CompanyName))
        {
            return this.BadRequest("CompanyName 輸入錯誤，必須輸入值");
        }

        if (string.IsNullOrWhiteSpace(parameter.Phone))
        {
            return this.BadRequest("Phone 輸入錯誤，必須輸入值");
        }

        if (shipperId.Equals(parameter.ShipperId) is false)
        {
            return this.BadRequest("輸入的 shipperId 與 parameter 的 ShipperId 不一致");
        }

        var exists = await this._shipperRepository.IsExistsAsync(shipperId);
        if (exists is false)
        {
            return this.BadRequest($"ShipperId: {shipperId} 所對映的資料不存在");
        }

        var model = await this._shipperRepository.GetAsync(shipperId);
        model.CompanyName = parameter.CompanyName.Trim();
        model.Phone = parameter.Phone.Trim();

        var result = await this._shipperRepository.UpdateAsync(model);
        if (result.Success is false)
        {
            return this.BadRequest("資料修改錯誤");
        }

        return this.Ok("資料修改完成");
    }

    /// <summary>
    /// 刪除 Shipper 資料
    /// </summary>
    /// <param name="shipperId">The ShipperId.</param>
    /// <returns></returns>
    [HttpDelete("{shipperId}")]
    public async Task<IActionResult> DeleteAsync(int shipperId)
    {
        if (shipperId <= 0)
        {
            return this.BadRequest("ShipperId 輸入錯誤，必須大於 0");
        }

        var exists = await this._shipperRepository.IsExistsAsync(shipperId);
        if (exists is false)
        {
            return this.BadRequest($"ShipperId: {shipperId} 所對映的資料不存在");
        }

        var result = await this._shipperRepository.DeleteAsync(shipperId);
        if (result.Success is false)
        {
            return this.BadRequest("資料刪除錯誤");
        }

        return this.Ok("資料刪除完成");
    }
}