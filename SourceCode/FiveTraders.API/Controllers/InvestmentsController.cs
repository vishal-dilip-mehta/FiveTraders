using System.ComponentModel.DataAnnotations;
using FiveTraders.RequestModel;
using FiveTraders.ResponseModel;
using FiveTraders.Service;
using Microsoft.AspNetCore.Mvc;

namespace FiveTraders.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class InvestmentsController(IInvestmentsService investmentsService)
    : ControllerBase
{
    [HttpGet("tickers")]
    public async Task<ActionResult<IReadOnlyList<TickerResponse>>> ReadTickers([FromQuery, Range(1, int.MaxValue)] int? tickerId = null,
                                                                               [FromQuery, StringLength(10)] string? tickerName = null,
                                                                               CancellationToken cancellationToken = default)
    {
        IReadOnlyList<TickerResponse> tickers = await investmentsService.ReadTickers(tickerId, tickerName, cancellationToken);
        return this.Ok(tickers);
    }

    [HttpPost("tickers")]
    public async Task<ActionResult<TickerResponse>> UpsertTicker([FromBody] TickerUpsertRequest request,
                                                                 CancellationToken cancellationToken = default)
    {
        TickerResponse ticker = await investmentsService.UpsertTicker(request, cancellationToken);
        return this.Ok(ticker);
    }

    [HttpGet("stock-transactions")]
    public async Task<ActionResult<IReadOnlyList<StockTransactionResponse>>> ReadStockTransactions([FromQuery, Range(1, int.MaxValue)] int? stockTransactionId = null,
                                                                                                   [FromQuery] int? stockTransactionLotId = null,
                                                                                                   [FromQuery, Range(1, int.MaxValue)] int? tickerId = null,
                                                                                                   CancellationToken cancellationToken = default)
    {
        IReadOnlyList<StockTransactionResponse> transactions = await investmentsService.ReadStockTransactions(stockTransactionId,
                                                                                                              stockTransactionLotId,
                                                                                                              tickerId,
                                                                                                              cancellationToken);
        return this.Ok(transactions);
    }

    [HttpPost("stock-transactions")]
    public async Task<ActionResult<StockTransactionResponse>> UpsertStockTransaction([FromBody] StockTransactionUpsertRequest request,
                                                                                     CancellationToken cancellationToken = default)
    {
        StockTransactionResponse transaction = await investmentsService.UpsertStockTransaction(request, cancellationToken);
        return this.Ok(transaction);
    }
}
