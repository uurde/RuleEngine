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

    [Theory]
    [InlineData(20)]
    [InlineData(70)]
    public void Returns10ForCustomerVeteran(int customerAge)
    {
        var customer = CreateCustomer(customerAge, DateTime.Today.AddDays(-1));
        customer.IsVeteran = true;
        var result = _discountCalculator.CalculateDiscountPercentage(customer);
        result.Should().Be(.10m);
    }

    [Theory]
    [InlineData(1, .05)]
    [InlineData(2, .08)]
    [InlineData(5, .10)]
    [InlineData(10, .12)]
    [InlineData(15, .15)]
    public void ReturnsCorrentLoyaltyDiscout(int yearsOfCustomer, decimal expectedDiscount)
    {
        var customer = CreateCustomer(DefaultAge, DateTime.Today.AddYears(-yearsOfCustomer).AddDays(-1));
        var result = _discountCalculator.CalculateDiscountPercentage(customer);
        result.Should().Be(expectedDiscount);
    }


    [Theory]
    [InlineData(1, .15)]
    [InlineData(2, .18)]
    [InlineData(5, .20)]
    [InlineData(10, .22)]
    [InlineData(15, .25)]
    public void ReturnsCorrentLoyaltyDiscoutWithBirthday(int yearsOfCustomer, decimal expectedDiscount)
    {
        var customer = CreateBirthdayCustomer(DefaultAge, DateTime.Today.AddYears(-yearsOfCustomer).AddDays(-1));
        var result = _discountCalculator.CalculateDiscountPercentage(customer);
        result.Should().Be(expectedDiscount);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void ReturnsVeteransDiscout1And2YearLoyalty(int yearsOfCustomer)
    {
        var customer = CreateCustomer(DefaultAge, DateTime.Today.AddYears(-yearsOfCustomer).AddDays(-1));
        customer.IsVeteran = true;
        var result = _discountCalculator.CalculateDiscountPercentage(customer);
        result.Should().Be(.10m);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void ReturnsVeteransDiscout1And2YearLoyaltyOnBirthday(int yearsOfCustomer)
    {
        var customer = CreateBirthdayCustomer(DefaultAge, DateTime.Today.AddYears(-yearsOfCustomer).AddDays(-1));
        customer.IsVeteran = true;
        var result = _discountCalculator.CalculateDiscountPercentage(customer);
        result.Should().Be(.20m);
    }

    [Fact]
    public void Returns10ForCustomerSecondPurchaseOnBirthday()
    {
        var customer = CreateBirthdayCustomer(20, DateTime.Today.AddDays(-1));
        var result = _discountCalculator.CalculateDiscountPercentage(customer);
        result.Should().Be(.10m);
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