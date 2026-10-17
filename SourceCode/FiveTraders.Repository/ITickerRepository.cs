using FiveTraders.DataModel;

namespace FiveTraders.Repository;

public interface ITickerRepository
{
    Task<IReadOnlyList<Ticker>> Read(int? tickerId = null,
                                        string? tickerName = null,
                                        CancellationToken cancellationToken = default);

    // An ID of zero inserts; a positive ID updates an existing row. Returns the saved row.
    Task<Ticker> Upsert(Ticker ticker, CancellationToken cancellationToken = default);
}
