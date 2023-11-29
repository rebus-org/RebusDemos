using Rebus.Sagas;

namespace Invoicing.Handlers;

public class InvoicingSagaData : SagaData
{
    public string TradeId { get; set; }
    public bool? Confirmed { get; set; }

    public string Commodity { get; set; }
    public decimal? Quantity { get; set; }

    public bool HasTradeDetails() => !string.IsNullOrWhiteSpace(Commodity);
}