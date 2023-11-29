﻿namespace Trading.Messages;

public record TradeRecorded
{
    public string TradeId { get; }
    public string Commodity { get; }
    public decimal Quantity { get; }

    public TradeRecorded(string tradeId, string commodity, decimal quantity)
    {
        TradeId = tradeId;
        Commodity = commodity;
        Quantity = quantity;
    }
}