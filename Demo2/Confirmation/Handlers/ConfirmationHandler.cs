using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Confirmation.Messages;
using Rebus.Bus;
using Rebus.Handlers;
using Trading.Messages;

namespace Confirmation.Handlers
{
    public class ConfirmationHandler : IHandleMessages<TradeCreated>
    {
        readonly HttpClient _httpClient;
        readonly IBus _bus;

        public ConfirmationHandler(IBus bus, HttpClient httpClient)
        {
            _bus = bus;
            _httpClient = httpClient;
        }

        public async Task Handle(TradeCreated message)
        {
            Console.Write($@"Received new trade with ID {message.TradeId}
    
    Counterparty: {message.Counterparty}
       Commodity: {message.Commodity}
        Quantity: {message.Quantity}

Checking ExternalCreditAssessor... ");

            var result = await _httpClient.GetStringAsync($"http://localhost:7000/check-credit?counterparty={message.Counterparty}");

            Console.WriteLine($"result: {result}");

            if (string.Equals(result, "true", StringComparison.CurrentCultureIgnoreCase))
            {
                await _bus.Publish(new TradeAccepted(message.TradeId));
            }
            else
            {
                await _bus.Publish(new TradeRejected(message.TradeId));
            }
        }
    }
}