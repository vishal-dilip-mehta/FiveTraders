using FiveTraders.RequestModel;
using FiveTraders.ResponseModel;

namespace FiveTraders.Service;

public interface IInvestmentsService
{
    Task<IReadOnlyList<TickerResponse>> ReadTickers(int? tickerId = null,
                                                  string? tickerName = null,
                                                  CancellationToken cancellationToken = default);

    Task<TickerResponse> UpsertTicker(TickerUpsertRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StockTransactionResponse>> ReadStockTransactions(int? stockTransactionId = null,
                                                                      int? stockTransactionLotId = null,
                                                                      int? tickerId = null,
                                                                      CancellationToken cancellationToken = default);

    Task<StockTransactionResponse> UpsertStockTransaction(StockTransactionUpsertRequest request,
                                                         CancellationToken cancellationToken = default);
}
