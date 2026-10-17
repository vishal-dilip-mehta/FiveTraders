using System.Data;
using FiveTraders.DataModel;
using Microsoft.Data.SqlClient;

namespace FiveTraders.Repository;

public sealed class StockTransactionRepository : IStockTransactionRepository
{
    private readonly string connectionString;

    public StockTransactionRepository(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        this.connectionString = connectionString;
    }

    public async Task<IReadOnlyList<StockTransaction>> Read(int? stockTransactionId = null,
                                                               int? stockTransactionLotId = null,
                                                               int? tickerId = null,
                                                               CancellationToken cancellationToken = default)
    {
        await using SqlConnection connection = new SqlConnection(this.connectionString);
        using SqlCommand command = new SqlCommand("Investments.StockTransactionRead", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@StockTransactionId", SqlDbType.Int).Value = (object?)stockTransactionId ?? DBNull.Value;
        command.Parameters.Add("@StockTransactionLotId", SqlDbType.Int).Value = (object?)stockTransactionLotId ?? DBNull.Value;
        command.Parameters.Add("@TickerId", SqlDbType.Int).Value = (object?)tickerId ?? DBNull.Value;

        await connection.OpenAsync(cancellationToken);
        await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
        List<StockTransaction> stockTransactions = [];
        while (await reader.ReadAsync(cancellationToken))
        {
            stockTransactions.Add(Map(reader));
        }

        return stockTransactions;
    }

    public async Task<StockTransaction> Upsert(StockTransaction stockTransaction,
                                                   CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stockTransaction);
        ArgumentOutOfRangeException.ThrowIfNegative(stockTransaction.StockTransactionId);

        await using SqlConnection connection = new SqlConnection(this.connectionString);
        using SqlCommand command = new SqlCommand("Investments.StockTransactionUpsert", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@StockTransactionId", SqlDbType.Int).Value =
            stockTransaction.StockTransactionId == 0 ? DBNull.Value : stockTransaction.StockTransactionId;
        command.Parameters.Add("@StockTransactionLotId", SqlDbType.Int).Value = stockTransaction.StockTransactionLotId;
        command.Parameters.Add("@TickerId", SqlDbType.Int).Value = stockTransaction.TickerId;
        command.Parameters.Add("@BuyDate", SqlDbType.DateTime2).Value = stockTransaction.BuyDate;
        command.Parameters.Add("@SellDate", SqlDbType.DateTime2).Value = (object?)stockTransaction.SellDate ?? DBNull.Value;
        AddDecimalParameter(command, "@BuyPrice", stockTransaction.BuyPrice);
        AddDecimalParameter(command, "@SellPrice", stockTransaction.SellPrice);
        AddDecimalParameter(command, "@Quantity", stockTransaction.Quantity);

        await connection.OpenAsync(cancellationToken);
        await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            throw new InvalidOperationException("StockTransactionUpsert did not return a stock transaction.");
        }

        return Map(reader);
    }

    private static void AddDecimalParameter(SqlCommand command, string name, decimal? value)
    {
        SqlParameter parameter = command.Parameters.Add(name, SqlDbType.Decimal);
        parameter.Precision = 19;
        parameter.Scale = 4;
        parameter.Value = (object?)value ?? DBNull.Value;
    }

    private static StockTransaction Map(SqlDataReader reader)
    {
        int sellDateOrdinal = reader.GetOrdinal(nameof(StockTransaction.SellDate));
        int sellPriceOrdinal = reader.GetOrdinal(nameof(StockTransaction.SellPrice));
        int profitOrdinal = reader.GetOrdinal(nameof(StockTransaction.Profit));
        int profitPercentOrdinal = reader.GetOrdinal(nameof(StockTransaction.ProfitPercent));

        return new StockTransaction
        {
            StockTransactionId = reader.GetInt32(reader.GetOrdinal(nameof(StockTransaction.StockTransactionId))),
            StockTransactionLotId = reader.GetInt32(reader.GetOrdinal(nameof(StockTransaction.StockTransactionLotId))),
            TickerId = reader.GetInt32(reader.GetOrdinal(nameof(StockTransaction.TickerId))),
            BuyDate = reader.GetDateTime(reader.GetOrdinal(nameof(StockTransaction.BuyDate))),
            BuyPrice = reader.GetDecimal(reader.GetOrdinal(nameof(StockTransaction.BuyPrice))),
            SellDate = reader.IsDBNull(sellDateOrdinal) ? null : reader.GetDateTime(sellDateOrdinal),
            SellPrice = reader.IsDBNull(sellPriceOrdinal) ? null : reader.GetDecimal(sellPriceOrdinal),
            Quantity = reader.GetDecimal(reader.GetOrdinal(nameof(StockTransaction.Quantity))),
            Profit = reader.IsDBNull(profitOrdinal) ? null : reader.GetDecimal(profitOrdinal),
            ProfitPercent = reader.IsDBNull(profitPercentOrdinal) ? null : reader.GetDecimal(profitPercentOrdinal),
        };
    }
}
