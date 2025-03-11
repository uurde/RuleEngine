namespace RuleEngine.First;

public interface IDiscountRule
{
    decimal CalculateDiscount(Customer customer, decimal currentDiscount);
}

public class FirstTimeCustomerRule : IDiscountRule
{
    public decimal CalculateDiscount(Customer customer, decimal currentDiscount)
    {
        if (!customer.DateOfFirstPurchase.HasValue)
        {
            return .15m;
        }
        return 0;
    }
}

public class LoyalCustomerRule : IDiscountRule
{
    public decimal CalculateDiscount(Customer customer, decimal currentDiscount)
    {
        if (customer.DateOfFirstPurchase.HasValue)
        {
            if (customer.DateOfFirstPurchase.Value < DateTime.Now.AddYears(-15))
            {
                return .15m;
            }
            if (customer.DateOfFirstPurchase.Value < DateTime.Now.AddYears(-10))
            {
                return .12m;
            }
            if (customer.DateOfFirstPurchase.Value < DateTime.Now.AddYears(-5))
            {
                return .10m;
            }
            if (customer.DateOfFirstPurchase.Value < DateTime.Now.AddYears(-2))
            {
                return .08m;
            }
            if (customer.DateOfFirstPurchase.Value < DateTime.Now.AddYears(-1))
            {
                return .05m;
            }
        }
        return 0;
    }
}

public class VeteranCustomerRule : IDiscountRule
{
    public decimal CalculateDiscount(Customer customer, decimal currentDiscount)
    {
        if (customer.IsVeteran)
        {
            return .10m;
        }
        return 0;
    }
}

public class SeniorCustomerRule : IDiscountRule
{
    public decimal CalculateDiscount(Customer customer, decimal currentDiscount)
    {
        if (customer.DateOfBirth < DateTime.Now.AddYears(-65))
        {
            return .05m;
        }
        return 0;
    }
}

public class BirthdayCustomerRule : IDiscountRule
{
    public decimal CalculateDiscount(Customer customer, decimal currentDiscount)
    {
        bool isBirthday = customer.DateOfBirth.HasValue && customer.DateOfBirth.Value.Month == DateTime.Now.Month && customer.DateOfBirth.Value.Day == DateTime.Now.Day;

        if (isBirthday)
        {
            return currentDiscount + 0.10m;
        }
        return currentDiscount;
    }
}

public class DiscountRuleEngine
{
    List<IDiscountRule> _rules = new List<IDiscountRule>();

    public DiscountRuleEngine(IEnumerable<IDiscountRule> rules)
    {
        _rules.AddRange(rules);
    }

    public decimal CalculateDiscountPercentage(Customer customer)
    {
        decimal discount = 0;
        foreach (var rule in _rules)
        {
            discount = Math.Max(discount, rule.CalculateDiscount(customer, discount));
        }
        return discount;
    }
}

public class DiscountCalculator
{
    public decimal CalculateDiscountPercentage(Customer customer)
    {
        var ruleType = typeof(IDiscountRule);
        IEnumerable<IDiscountRule> rules = this.GetType().Assembly.GetTypes()
            .Where(t => ruleType.IsAssignableFrom(t) && !t.IsInterface)
            .Select(t => (IDiscountRule)Activator.CreateInstance(t) as IDiscountRule);
        
        var engine = new DiscountRuleEngine(rules);
        
        return engine.CalculateDiscountPercentage(customer);
    }
}