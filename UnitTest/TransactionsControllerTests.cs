using AngularApp.Server.Data;
using AngularApp.Server.Models;  
using AngularApp.Server.Services;
using Microsoft.EntityFrameworkCore;

public class TransactionServiceTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TransactionDbTest")
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddOrUpdateTransactionAsync_AddsNewTransaction_WhenNotExists()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new TransactionService(context);

        var transaction = new Transaction
        {
            TransactionId = 1,
            TradeId = 100,
            Version = 1,
            SecurityCode = "ABC",
            Quantity = 10,
            Action = "INSERT",
            BuySell = "BUY"
        };

        // Act
        await service.AddOrUpdateTransactionAsync(transaction);

        // Assert
        var saved = await context.Transactions.FirstOrDefaultAsync(t => t.TradeId == 100 && t.Version == 1);
        Assert.NotNull(saved);
        Assert.Equal("ABC", saved.SecurityCode);
        Assert.Equal(10, saved.Quantity);
    }

    [Fact]
    public async Task AddOrUpdateTransactionAsync_UpdatesExistingTransaction_WhenExists()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new TransactionService(context);

        var existingTransaction = new Transaction
        {
            TransactionId = 1,
            TradeId = 100,
            Version = 1,
            SecurityCode = "ABC",
            Quantity = 10,
            Action = "INSERT",
            BuySell = "BUY"
        };
        await context.Transactions.AddAsync(existingTransaction);
        await context.SaveChangesAsync();

        var newTransaction = new Transaction
        {
            TransactionId = 2,
            TradeId = 100,
            Version = 1,
            SecurityCode = "XYZ",
            Quantity = 20,
            Action = "UPDATE",
            BuySell = "SELL"
        };

        // Act
        await service.AddOrUpdateTransactionAsync(newTransaction);

        // Assert
        var transactions = await context.Transactions
            .Where(t => t.TradeId == 100 && t.Version == 1)
            .ToListAsync();

        Assert.Single(transactions);
        Assert.Equal("XYZ", transactions[0].SecurityCode);
        Assert.Equal(20, transactions[0].Quantity);
        Assert.Equal("SELL", transactions[0].BuySell);
    }

    [Fact]
    public async Task GetPositionsAsync_ReturnsAggregatedPositions_ExcludingCancelled()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var service = new TransactionService(context);

        // Setup transactions with multiple versions and actions
        var transactions = new List<Transaction>
        {
            new Transaction { TransactionId = 1, TradeId = 1, Version = 1, SecurityCode = "AAA", Quantity = 10, Action = "INSERT", BuySell = "BUY" },
            new Transaction { TransactionId = 2, TradeId = 1, Version = 2, SecurityCode = "AAA", Quantity = 15, Action = "UPDATE", BuySell = "BUY" },
            new Transaction { TransactionId = 3, TradeId = 2, Version = 1, SecurityCode = "BBB", Quantity = 20, Action = "INSERT", BuySell = "SELL" },
            new Transaction { TransactionId = 4, TradeId = 3, Version = 1, SecurityCode = "AAA", Quantity = 5, Action = "CANCEL", BuySell = "BUY" }, // should be excluded
            new Transaction { TransactionId = 5, TradeId = 2, Version = 2, SecurityCode = "BBB", Quantity = 10, Action = "UPDATE", BuySell = "SELL" },
        };
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync();

        // Act
        var positions = await service.GetPositionsAsync();

        // Assert
        // Latest versions are: TradeId 1 -> Version 2, TradeId 2 -> Version 2, TradeId 3 -> Version 1 (cancelled so excluded)
        Assert.Equal(2, positions.Count);

        var posAAA = positions.SingleOrDefault(p => p.SecurityCode == "AAA");
        var posBBB = positions.SingleOrDefault(p => p.SecurityCode == "BBB");

        Assert.NotNull(posAAA);
        Assert.Equal(15, posAAA.Quantity);  // BUY 15

        Assert.NotNull(posBBB);
        Assert.Equal(-10, posBBB.Quantity); // SELL 10 -> quantity negative because Sell
    }
}
