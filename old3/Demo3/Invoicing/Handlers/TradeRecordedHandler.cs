using System;
using System.Threading.Tasks;
using Rebus.Handlers;
using Trading.Messages;

namespace Invoicing.Handlers
{
    public class TradeRecordedHandler : IHandleMessages<TradeRecorded>
    {
        public async Task Handle(TradeRecorded message)
        {
            Console.WriteLine($@"Invoicing trade: {message.TradeId} ({message.Quantity} x {message.Commodity})");
        }
    }
}