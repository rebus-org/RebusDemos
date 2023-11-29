namespace Confirmation.Messages;

public class TradeApproved
{
    public string TradeId { get; }

    public TradeApproved(string tradeId)
    {
        TradeId = tradeId;
    }
}