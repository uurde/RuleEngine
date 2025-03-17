using RuleEngine.App.Models;

namespace RuleEngine.App;

public interface IDataAccess
{
    Task<List<User>> GetUsers();
    Task<List<CouponModel>> GetCoupons();
}