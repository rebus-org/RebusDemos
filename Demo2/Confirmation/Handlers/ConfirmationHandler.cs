using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Confirmation.Messages;
using Rebus.Bus;
using Rebus.Handlers;
using Trading.Messages;

namespace Confirmation.Handlers
{
    public class ConfirmationHandler : IHandleMessages<TradeCreated>
    {
        readonly HttpClient _creditAssessmentClient;
        readonly IBus _bus;

        public ConfirmationHandler(IBus bus, HttpClient creditAssessmentClient)
        {
            _bus = bus;
            _creditAssessmentClient = creditAssessmentClient;
        }

        public async Task Handle(TradeCreated message)
        {
            Console.Write($@"Received new trade with ID {message.TradeId}
    
    Counterparty: {message.Counterparty}
       Commodity: {message.Commodity}
        Quantity: {message.Quantity}

Checking ExternalCreditAssessor... ");

            var counterparty = HttpUtility.UrlEncode(message.Counterparty);
            var result = await _creditAssessmentClient.GetStringAsync($"check-credit?counterparty={counterparty}");

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