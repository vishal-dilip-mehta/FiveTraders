using System.ComponentModel.DataAnnotations;

namespace FiveTraders.RequestModel;

public sealed class StockTransactionUpsertRequest
{
    [Range(0, int.MaxValue)]
    public int StockTransactionId { get; set; }

    public required int StockTransactionLotId { get; set; }

    [Range(1, int.MaxValue)]
    public required int TickerId { get; set; }

    public required DateTime BuyDate { get; set; }

    public required decimal BuyPrice { get; set; }

    public DateTime? SellDate { get; set; }

    public decimal? SellPrice { get; set; }

    public required decimal Quantity { get; set; }
}
