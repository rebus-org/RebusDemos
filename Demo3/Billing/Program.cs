using System;
using System.Threading.Tasks;
using Billing.Handlers;
using Common;
using Confirmation.Messages;
using Rebus.Activation;
using Rebus.Config;
using Trading.Messages;

// ReSharper disable ArgumentsStyleLiteral

namespace Billing
{
    class Program
    {
        static void Main()
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                activator
                    .Register((bus, context) => new TradeCreatedSaga(bus))
                    .Register(() => new SendInvoiceHandler())
                    .Register(() => new TakeOutForManualInspectionHandler());

                Configure.With(activator)
                    .ConfigureEndpoint(Config.Queues.Billing)
                    .Start();

                Task.WaitAll(
                    activator.Bus.Subscribe<TradeCreated>(),
                    activator.Bus.Subscribe<TradeAccepted>(),
                    activator.Bus.Subscribe<TradeRejected>()
                );

                Console.WriteLine("===== Billing =====");

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
