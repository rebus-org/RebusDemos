namespace Billing.Messages
{
    public class TakeOutForManualInspection
    {
        public string TradeId { get; }

        public TakeOutForManualInspection(string tradeId)
        {
            TradeId = tradeId;
        }
    }
}