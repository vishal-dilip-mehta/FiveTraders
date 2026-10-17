namespace FiveTraders.DataModel;

public sealed class Ticker
{
    public int TickerId { get; set; }

    public required string TickerName { get; set; }
}
