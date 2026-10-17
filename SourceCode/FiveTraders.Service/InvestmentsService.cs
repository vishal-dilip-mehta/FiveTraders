using FiveTraders.DataModel;
using FiveTraders.Repository;
using FiveTraders.RequestModel;
using FiveTraders.ResponseModel;

namespace FiveTraders.Service;

public sealed class InvestmentsService(ITickerRepository tickerRepository,
                                       IStockTransactionRepository stockTransactionRepository)
    : IInvestmentsService
{
    public async Task<IReadOnlyList<TickerResponse>> ReadTickers(int? tickerId = null,
                                                                 string? tickerName = null,
                                                                 CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Ticker> tickers = await tickerRepository.Read(tickerId, tickerName, cancellationToken);
        return tickers.Select(Map).ToArray();
    }

    public async Task<TickerResponse> UpsertTicker(TickerUpsertRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        Ticker ticker = new()
        {
            TickerId = request.TickerId,
            TickerName = request.TickerName,
        };

        Ticker savedTicker = await tickerRepository.Upsert(ticker, cancellationToken);
        return Map(savedTicker);
    }

    public async Task<IReadOnlyList<StockTransactionResponse>> ReadStockTransactions(int? stockTransactionId = null,
                                                                                     int? stockTransactionLotId = null,
                                                                                     int? tickerId = null,
                                                                                     CancellationToken cancellationToken = default)
    {
        IReadOnlyList<StockTransaction> transactions = await stockTransactionRepository.Read(stockTransactionId,
                                                                                             stockTransactionLotId,
                                                                                             tickerId,
                                                                                             cancellationToken);
        return transactions.Select(Map).ToArray();
    }

    public async Task<StockTransactionResponse> UpsertStockTransaction(StockTransactionUpsertRequest request,
                                                                       CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        StockTransaction transaction = new()
        {
            StockTransactionId = request.StockTransactionId,
            StockTransactionLotId = request.StockTransactionLotId,
            TickerId = request.TickerId,
            BuyDate = request.BuyDate,
            BuyPrice = request.BuyPrice,
            SellDate = request.SellDate,
            SellPrice = request.SellPrice,
            Quantity = request.Quantity,
        };

        StockTransaction savedTransaction = await stockTransactionRepository.Upsert(transaction, cancellationToken);
        return Map(savedTransaction);
    }

    private static TickerResponse Map(Ticker ticker)
    {
        return new TickerResponse
        {
            TickerId = ticker.TickerId,
            TickerName = ticker.TickerName,
        };
    }

    private static StockTransactionResponse Map(StockTransaction transaction)
    {
        return new StockTransactionResponse
        {
            StockTransactionId = transaction.StockTransactionId,
            StockTransactionLotId = transaction.StockTransactionLotId,
            TickerId = transaction.TickerId,
            BuyDate = transaction.BuyDate,
            BuyPrice = transaction.BuyPrice,
            SellDate = transaction.SellDate,
            SellPrice = transaction.SellPrice,
            Quantity = transaction.Quantity,
            Profit = transaction.Profit,
            ProfitPercent = transaction.ProfitPercent,
        };
    }
}
