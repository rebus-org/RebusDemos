using System;
using System.Threading.Tasks;
using Billing.Messages;
using Rebus.Handlers;

namespace Billing.Handlers
{
    public class SendInvoiceHandler : IHandleMessages<SendInvoice>
    {
        public async Task Handle(SendInvoice message)
        {
            Console.WriteLine($"Sending invoice to {message.Counterparty}");
        }
    }
}