using System;
using System.Threading.Tasks;
using Rebus.Handlers;
using Trading.Messages;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Invoicing.Handlers;

public class TradeRecordedHandler : IHandleMessages<TradeRecorded>
{
    public async Task Handle(TradeRecorded message)
    {
        Console.WriteLine($"Invoicing trade: {message.TradeId} ({message.Quantity} x {message.Commodity})");
    }
}