using Microsoft.EntityFrameworkCore;

namespace RuleEngine.App;

public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SupportedTeam { get; set; }
    public DateTime DateOfRegistiration { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateOfBirthday { get; set; }
}

public class Coupon
{
    public string CouponId { get; set; }
    public int MemberId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PlayedDate { get; set; }
}