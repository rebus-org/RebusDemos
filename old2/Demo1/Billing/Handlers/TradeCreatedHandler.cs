using System;
using System.Threading.Tasks;
using Billing.Messages;
using Rebus.Bus;
using Rebus.Handlers;
using Trading.Messages;

namespace Billing.Handlers
{
    public class TradeCreatedHandler : IHandleMessages<TradeCreated>
    {
        readonly IBus _bus;

        public TradeCreatedHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(TradeCreated message)
        {
            Console.WriteLine($"New trade created: {message.TradeId}");

            await _bus.SendLocal(new SendInvoice(message.Counterparty));
        }
    }
}