using System;
using Sample.WebApplication.Infrastructure.Models;
using IResult = Sample.WebApplication.Infrastructure.Misc.IResult;

namespace Sample.WebApplication.Infrastructure.Repository;

/// <summary>
/// interface IShipperRepository
/// </summary>
public interface IShipperRepository
{
    /// <summary>
    /// 以 ShipperId 查詢資料是否存在
    /// </summary>
    /// <param name="shipperId">shipperId</param>
    /// <returns></returns>
    Task<bool> IsExistsAsync(int shipperId);

    /// <summary>
    /// 以 ShipperId 取得資料
    /// </summary>
    /// <param name="shipperId">shipperId</param>
    /// <returns></returns>
    Task<ShipperModel> GetAsync(int shipperId);

    /// <summary>
    /// 取得 Shipper 的資料總數
    /// </summary>
    /// <returns></returns>
    Task<int> GetTotalCountAsync();

    /// <summary>
    /// 取得所有 Shipper 資料
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ShipperModel>> GetAllAsync();

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    Task<IResult> CreateAsync(ShipperModel model);

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    Task<IResult> UpdateAsync(ShipperModel model);

    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="shipperId">shipperId</param>
    /// <returns></returns>
    Task<IResult> DeleteAsync(int shipperId);
}