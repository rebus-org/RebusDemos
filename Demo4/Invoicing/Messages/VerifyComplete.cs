namespace Invoicing.Messages
{
    public class VerifyComplete
    {
        public string TradeId { get; }

        public VerifyComplete(string tradeId)
        {
            TradeId = tradeId;
        }    
    }
}