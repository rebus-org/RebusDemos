namespace TradeClearanceHouseApi.Dtos
{
    class CheckTradeRequest
    {
        public string TradeId { get; }
        public string Commodity { get; }
        public decimal Quantity { get; }

        public CheckTradeRequest(string tradeId, string commodity, decimal quantity)
        {
            TradeId = tradeId;
            Commodity = commodity;
            Quantity = quantity;
        }
    }
}