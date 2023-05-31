namespace GroupFlights.Sales.Infrastructure.Data.Models;

internal abstract class DbModel
{
    public Guid Version { get; set; }
    public DateTime CreateDateUtc { get; set; }
    public DateTime UpdateDateUtc { get; set; }
}