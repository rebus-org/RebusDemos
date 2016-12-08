namespace Trading.Messages
{
    public class TradeCreated
    {
        public string Counterparty { get; }
        public string TradeId { get; }
        public string Commodity { get; }
        public decimal Quantity { get; }

        public TradeCreated(string tradeId, string counterparty, string commodity, decimal quantity)
        {
            TradeId = tradeId;
            Commodity = commodity;
            Counterparty = counterparty;
            Quantity = quantity;
        }
    }
}
