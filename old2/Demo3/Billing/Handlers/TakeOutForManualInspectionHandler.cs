using System;
using System.Threading.Tasks;
using Billing.Messages;
using Rebus.Handlers;

namespace Billing.Handlers
{
    public class TakeOutForManualInspectionHandler : IHandleMessages<TakeOutForManualInspection>
    {
        public async Task Handle(TakeOutForManualInspection message)
        {
            Console.WriteLine($"Inspecting trade {message.TradeId} manually");
        }
    }
}