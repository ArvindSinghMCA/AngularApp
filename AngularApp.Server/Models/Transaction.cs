namespace AngularApp.Server.Models;

public class Transaction
{
    public int TransactionId { get; set; } // PK, auto-generated
    public int TradeId { get; set; }
    public int Version { get; set; }
    public string SecurityCode { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Action { get; set; } = string.Empty; // INSERT, UPDATE, CANCEL
    public string BuySell { get; set; } = string.Empty; // BUY, SELL
}

