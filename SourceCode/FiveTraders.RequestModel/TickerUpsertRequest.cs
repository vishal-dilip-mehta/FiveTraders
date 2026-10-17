using System.ComponentModel.DataAnnotations;

namespace FiveTraders.RequestModel;

public sealed class TickerUpsertRequest
{
    [Range(0, int.MaxValue)]
    public int TickerId { get; set; }

    [Required]
    [StringLength(10)]
    public required string TickerName { get; set; }
}
