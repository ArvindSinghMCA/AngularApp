using AngularApp.Server.Data;
using AngularApp.Server.Models;
using Microsoft.EntityFrameworkCore;
 

namespace AngularApp.Server.Services;
public class TransactionService : ITransactionService
{
    private readonly AppDbContext _context;

    public TransactionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddOrUpdateTransactionAsync(Transaction transaction)
    {
        var existing = await _context.Transactions
            .FirstOrDefaultAsync(t => t.TradeId == transaction.TradeId && t.Version == transaction.Version);

        if (existing != null)
        {
            _context.Transactions.Remove(existing);
        }

        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Position>> GetPositionsAsync()
    {
        // Get the max version per TradeId
        var maxVersions = _context.Transactions
            .GroupBy(t => t.TradeId)
            .Select(g => new { TradeId = g.Key, MaxVersion = g.Max(t => t.Version) });

        // Join to get latest transactions per TradeId
        var latestVersions = await (from t in _context.Transactions
                                    join mv in maxVersions
                                      on new { t.TradeId, t.Version } equals new { mv.TradeId, Version = mv.MaxVersion }
                                    where t.Action.ToUpper() != "CANCEL"
                                    select t)
                                    .ToListAsync();

        // Aggregate positions by SecurityCode
        var positions = latestVersions
            .GroupBy(t => t.SecurityCode)
            .Select(g => new Position
            {
                SecurityCode = g.Key,
                Quantity = g.Sum(t => (t.BuySell.ToUpper() == "BUY" ? 1 : -1) * t.Quantity)
            })
            .ToList();

        return positions;
    }
}
