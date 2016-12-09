namespace Billing.Messages
{
    public class TradeSagaTimeout
    {
        public string TradeId { get; }

        public TradeSagaTimeout(string tradeId)
        {
            TradeId = tradeId;
        }
    }
}