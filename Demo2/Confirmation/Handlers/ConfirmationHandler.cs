using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confirmation.Messages;
using Rebus.Bus;
using Rebus.Handlers;
using Trading.Messages;

namespace Confirmation.Handlers
{
    public class ConfirmationHandler : IHandleMessages<TradeCreated>
    {
        readonly IBus _bus;

        public ConfirmationHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(TradeCreated message)
        {
            Console.WriteLine($@"Received new trade with ID {message.TradeId}
    
    Counterparty: {message.Counterparty}
       Commodity: {message.Commodity}
        Quantity: {message.Quantity}

Would you like to accept this trade? (y/n)");

            var key = ReadKey("yn");

            if (key == 'y')
            {
                await _bus.Publish(new TradeAccepted(message.TradeId));
            }
            else
            {
                await _bus.Publish(new TradeRejected(message.TradeId));
            }
        }

        static char ReadKey(IEnumerable<char> acceptedKeys)
        {
            var keysHash = new HashSet<char>(acceptedKeys.Select(char.ToLower));
            while (true)
            {
                var keyChar = char.ToLower(Console.ReadKey(true).KeyChar);
                if (!keysHash.Contains(keyChar)) continue;
                return keyChar;
            }
        }
    }
}