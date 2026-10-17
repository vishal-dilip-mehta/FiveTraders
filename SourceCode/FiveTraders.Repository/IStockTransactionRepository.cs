using FiveTraders.DataModel;

namespace FiveTraders.Repository;

public interface IStockTransactionRepository
{
    Task<IReadOnlyList<StockTransaction>> Read(int? stockTransactionId = null,
                                                  int? stockTransactionLotId = null,
                                                  int? tickerId = null,
                                                  CancellationToken cancellationToken = default);

    // An ID of zero inserts; a positive ID updates an existing row. Returns the saved row, including computed profits.
    Task<StockTransaction> Upsert(StockTransaction stockTransaction, CancellationToken cancellationToken = default);
}
