using System.Data;
using FiveTraders.DataModel;
using Microsoft.Data.SqlClient;

namespace FiveTraders.Repository;

public sealed class TickerRepository : ITickerRepository
{
    private readonly string connectionString;

    public TickerRepository(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        this.connectionString = connectionString;
    }

    public async Task<IReadOnlyList<Ticker>> Read(int? tickerId = null,
                                                     string? tickerName = null,
                                                     CancellationToken cancellationToken = default)
    {
        if (tickerName is { Length: > 10 })
        {
            throw new ArgumentException("Ticker names cannot exceed 10 characters.", nameof(tickerName));
        }

        await using SqlConnection connection = new(this.connectionString);
        using SqlCommand command = new("Investments.TickerRead", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@TickerId", SqlDbType.Int).Value = (object?)tickerId ?? DBNull.Value;
        // The existing stored procedure names this parameter TicketName.
        command.Parameters.Add("@TicketName", SqlDbType.NVarChar, 10).Value = (object?)tickerName ?? DBNull.Value;

        await connection.OpenAsync(cancellationToken);
        await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
        List<Ticker> tickers = [];
        while (await reader.ReadAsync(cancellationToken))
        {
            tickers.Add(Map(reader));
        }

        return tickers;
    }

    public async Task<Ticker> Upsert(Ticker ticker, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ticker);
        ArgumentOutOfRangeException.ThrowIfNegative(ticker.TickerId);
        ArgumentException.ThrowIfNullOrWhiteSpace(ticker.TickerName);
        if (ticker.TickerName.Length > 10)
        {
            throw new ArgumentException("Ticker names cannot exceed 10 characters.", nameof(ticker));
        }

        await using SqlConnection connection = new(this.connectionString);
        using SqlCommand command = new("Investments.TickerUpsert", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@TickerId", SqlDbType.Int).Value = ticker.TickerId == 0 ? DBNull.Value : ticker.TickerId;
        command.Parameters.Add("@TicketName", SqlDbType.NVarChar, 10).Value = ticker.TickerName;

        await connection.OpenAsync(cancellationToken);
        await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            throw new InvalidOperationException("TickerUpsert did not return a ticker.");
        }

        return Map(reader);
    }

    private static Ticker Map(SqlDataReader reader)
    {
        return new Ticker
        {
            TickerId = reader.GetInt32(reader.GetOrdinal(nameof(Ticker.TickerId))),
            TickerName = reader.GetString(reader.GetOrdinal(nameof(Ticker.TickerName))),
        };
    }
}
