namespace Confirmation.Messages;

public class TradeRejected
{
    public string TradeId { get; }

    public TradeRejected(string tradeId)
    {
        TradeId = tradeId;
    }
}