// Data/DbInitializer.cs
using AngularApp.Server.Data;
using AngularApp.Server.Models;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        if (context.Transactions.Any()) return; // Don't seed if already seeded

        var transactions = new List<Transaction>
        {
            new() { TradeId = 1, Version = 1, SecurityCode = "REL", Quantity = 50, Action = "INSERT", BuySell = "Buy" },
            new() { TradeId = 2, Version = 1, SecurityCode = "ITC", Quantity = 40, Action = "INSERT", BuySell = "Buy" },
            new() { TradeId = 3, Version = 1, SecurityCode = "INF", Quantity = 70, Action = "INSERT", BuySell = "Sell" },
            new() { TradeId = 1, Version = 2, SecurityCode = "REL", Quantity = 60, Action = "INSERT", BuySell = "Buy" },
            new() { TradeId = 2, Version = 2, SecurityCode = "ITC", Quantity = 30, Action = "UPDATE", BuySell = "Buy" },
            new() { TradeId = 4, Version = 1, SecurityCode = "INF", Quantity = 20, Action = "CANCEL", BuySell = "Buy" },
        };

        context.Transactions.AddRange(transactions);
        context.SaveChanges();
    }
}
