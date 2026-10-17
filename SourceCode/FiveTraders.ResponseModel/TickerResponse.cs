namespace FiveTraders.ResponseModel;

public sealed class TickerResponse
{
    public int TickerId { get; init; }

    public required string TickerName { get; init; }
}
