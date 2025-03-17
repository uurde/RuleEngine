using Npgsql;
using System.Data;
using Dapper;
using RuleEngine.App.Models;

namespace RuleEngine.App;

public class DataAccess : BaseRepository, IDataAccess
{
    public DataAccess(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<List<User>> GetUsers()
    {
        using var connection = GetDbConnection().WaitAsync(cancellationToken: CancellationToken.None).GetAwaiter()
            .GetResult();
        string query =
            "SELECT \"UserId\", \"FirstName\", \"LastName\", \"SupportedTeam\", \"DateOfRegistiration\", \"DateOfBirthday\" FROM public.\"User\";";
        var users = await connection.QueryAsync<User>(query);
        return users.ToList();
    }

    public async Task<List<CouponModel>> GetCoupons()
    {
        using var connection = GetDbConnection().WaitAsync(cancellationToken: CancellationToken.None).GetAwaiter()
            .GetResult();
        string query = "SELECT c.\"CouponId\", u.\"FirstName\" || ' ' || u.\"LastName\" AS \"User\", " +
                       "c.\"Amount\", c.\"PlayedDate\" " +
                       "FROM public.\"Coupon\" c " +
                       "INNER JOIN public.\"User\" u ON c.\"UserId\" = u.\"UserId\";";

        var coupons = await connection.QueryAsync<CouponModel>(query);
        return coupons.ToList();
    }
}