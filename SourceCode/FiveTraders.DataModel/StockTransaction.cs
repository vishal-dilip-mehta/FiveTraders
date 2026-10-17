namespace FiveTraders.DataModel;

public sealed class StockTransaction
{
    public int StockTransactionId { get; set; }

    public int StockTransactionLotId { get; set; }

    public int TickerId { get; set; }

    public DateTime BuyDate { get; set; }

    public decimal BuyPrice { get; set; }

    public DateTime? SellDate { get; set; }

    public decimal? SellPrice { get; set; }

    public decimal Quantity { get; set; }

    // Calculated by SQL Server and returned by the read/upsert procedures.
    public decimal? Profit { get; init; }

    public decimal? ProfitPercent { get; init; }
}
