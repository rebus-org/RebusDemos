namespace Confirmation.Messages
{
    public class TradeAccepted
    {
        public string TradeId { get; }

        public TradeAccepted(string tradeId)
        {
            TradeId = tradeId;
        }
    }
}
