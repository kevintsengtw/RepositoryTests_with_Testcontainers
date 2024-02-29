using Dapper;
using Sample.WebApplication.Infrastructure.Helpers;
using Sample.WebApplication.Infrastructure.Misc;
using Sample.WebApplication.Infrastructure.Models;
using Throw;
using IResult = Sample.WebApplication.Infrastructure.Misc.IResult;

namespace Sample.WebApplication.Infrastructure.Repository;

/// <summary>
/// class ShipperRepository
/// </summary>
/// <seealso cref="IShipperRepository" />
public class ShipperRepository : IShipperRepository
{
    private readonly IDatabaseHelper _databaseHelper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShipperRepository"/> class.
    /// </summary>
    /// <param name="databaseHelper">The database helper.</param>
    public ShipperRepository(IDatabaseHelper databaseHelper)
    {
        this._databaseHelper = databaseHelper;
    }

    /// <summary>
    /// 以 ShipperId 查詢資料是否存在
    /// </summary>
    /// <param name="shipperId">shipperId</param>
    /// <returns></returns>
    public async Task<bool> IsExistsAsync(int shipperId)
    {
        shipperId.Throw().IfLessThanOrEqualTo(0);
        
        using var conn = this._databaseHelper.GetConnection();

        const string sqlCommand = """
                                  select count(ShipperId) from Shippers
                                  where ShipperId = @ShipperId
                                  """;

        var parameters = new DynamicParameters();
        parameters.Add("ShipperId", shipperId);

        var result = await conn.QueryFirstOrDefaultAsync<int>(sqlCommand, parameters);
        return result > 0;
    }

    /// <summary>
    /// 以 ShipperId 取得資料
    /// </summary>
    /// <param name="shipperId">shipperId</param>
    /// <returns></returns>
    public async Task<ShipperModel> GetAsync(int shipperId)
    {
        shipperId.Throw().IfLessThanOrEqualTo(0);
        
        using var conn = this._databaseHelper.GetConnection();

        const string sqlCommand = """
                                  select ShipperId, CompanyName, Phone from Shippers
                                  where ShipperId = @ShipperId
                                  """;

        var parameters = new DynamicParameters();
        parameters.Add("ShipperId", shipperId);

        var result = await conn.QueryFirstOrDefaultAsync<ShipperModel>(sqlCommand, parameters);
        return result;
    }

    /// <summary>
    /// 取得 Shipper 的資料總數
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetTotalCountAsync()
    {
        using var conn = this._databaseHelper.GetConnection();
        const string sqlCommand = @"select count(ShipperId) from Shippers ";
        var result = await conn.QueryFirstOrDefaultAsync<int>(sqlCommand);
        return result;
    }

    /// <summary>
    /// 取得所有 Shipper 資料
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<ShipperModel>> GetAllAsync()
    {
        using var conn = this._databaseHelper.GetConnection();

        const string sqlCommand = """
                                  select ShipperId, CompanyName, Phone from Shippers
                                  order by ShipperId ASC
                                  """;

        var result = await conn.QueryAsync<ShipperModel>(sqlCommand);
        return result;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    public async Task<IResult> CreateAsync(ShipperModel model)
    {
        model.ThrowIfNull();
        
        using var conn = this._databaseHelper.GetConnection();

        const string sqlCommand = """
                                  Insert into Shippers (CompanyName, Phone)
                                  Values (@CompanyName, @Phone)
                                  """;

        var executeResult = await conn.ExecuteAsync(sqlCommand, model);

        var result = new Result(false);

        if (executeResult.Equals(1))
        {
            result.Success = true;
            result.AffectRows = executeResult;
            return result;
        }

        result.Message = "資料新增錯誤";
        return result;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    public async Task<IResult> UpdateAsync(ShipperModel model)
    {
        model.ThrowIfNull();
        
        using var conn = this._databaseHelper.GetConnection();
        
        const string sqlCommand = """
                                  UPDATE Shippers SET
                                  CompanyName = @CompanyName,
                                  Phone = @Phone
                                  WHERE ShipperID = @ShipperID
                                  """;

        var executeResult = await conn.ExecuteAsync(sqlCommand, model);

        IResult result = new Result(false);

        if (executeResult.Equals(1))
        {
            result.Success = true;
            result.AffectRows = executeResult;
            return result;
        }

        result.Message = "資料更新錯誤";
        return result;
    }

    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="shipperId">shipperId</param>
    /// <returns></returns>
    public async Task<IResult> DeleteAsync(int shipperId)
    {
        shipperId.Throw().IfLessThanOrEqualTo(0);
        
        using var conn = this._databaseHelper.GetConnection();
        
        const string sqlCommand = "DELETE FROM Shippers WHERE ShipperID = @ShipperID";

        var parameters = new DynamicParameters();
        parameters.Add("ShipperID", shipperId);

        var executeResult = await conn.ExecuteAsync(sqlCommand, parameters);

        IResult result = new Result(false);

        if (executeResult.Equals(1))
        {
            result.Success = true;
            result.AffectRows = executeResult;
            return result;
        }

        result.Message = "資料刪除錯誤";
        return result;
    }
}