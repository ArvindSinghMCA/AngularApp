using AngularApp.Server.Models;

public interface ITransactionService
{
    Task AddOrUpdateTransactionAsync(Transaction transaction);
    Task<List<Position>> GetPositionsAsync();
}
