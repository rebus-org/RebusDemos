namespace Billing.Messages
{
    public class SendInvoice
    {
        public string Counterparty { get; }
        public string TradeId { get; }

        public SendInvoice(string counterparty, string tradeId)
        {
            Counterparty = counterparty;
            TradeId = tradeId;
        }
    }
}