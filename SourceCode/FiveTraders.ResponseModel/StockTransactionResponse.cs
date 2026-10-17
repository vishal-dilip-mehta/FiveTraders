namespace FiveTraders.ResponseModel;

public sealed class StockTransactionResponse
{
    public int StockTransactionId { get; init; }

    public int StockTransactionLotId { get; init; }

    public int TickerId { get; init; }

    public DateTime BuyDate { get; init; }

    public decimal BuyPrice { get; init; }

    public DateTime? SellDate { get; init; }

    public decimal? SellPrice { get; init; }

    public decimal Quantity { get; init; }

    public decimal? Profit { get; init; }

    public decimal? ProfitPercent { get; init; }
}
