using FluentAssertions;
using Xunit;

namespace RuleEngine.First;

public class DiscountCalculatorTest
{
    private DiscountCalculator _discountCalculator = new DiscountCalculator();
    const int DefaultAge = 30;

    [Fact]
    public void Returns0ForBasicCustomer()
    {
        var customer = CreateCustomer(DefaultAge, DateTime.Today.AddDays(-1));
        var result = _discountCalculator.CalculateDiscountPercentage(customer);
        result.Should().Be(0m);
    }

    [Fact]
    public void Returns5ForCustomersOver65()
    {
        var customer = CreateCustomer(65, DateTime.Today.AddDays(-1));
        var result = _discountCalculator.CalculateDiscountPercentage(customer);
        result.Should().Be(.05m);
    }

    [Theory]
    [InlineData(20)]
    [InlineData(70)]
    public void Returns15ForCustomerFirstPurchase(int customerAge)
    {
        var customer = CreateCustomer(customerAge);
        var result = _discountCalculator.CalculateDiscountPercentage(customer);
        result.Should().Be(.15m);
    }

    private Customer CreateCustomer(int age = DefaultAge, DateTime? firstPurchaseDate = null)
    {
        return new Customer
        {
            DateOfBirth = DateTime.Today.AddYears(-age).AddDays(-1),
            DateOfFirstPurchase = firstPurchaseDate
        };
    }

    private Customer CreateBirthdayCustomer(int age = DefaultAge, DateTime? firstPurchaseDate = null)
    {
        return new Customer
        {
            DateOfBirth = DateTime.Today.AddYears(-age),
            DateOfFirstPurchase = firstPurchaseDate
        };
    }
}