using System;
using Billing.Handlers;
using Common;
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
                    .Register((bus, context) => new TradeCreatedHandler(bus))
                    .Register(() => new SendInvoiceHandler());

                Configure.With(activator)
                    .ConfigureEndpoint("billing")
                    .Start();

                activator.Bus.Subscribe<TradeCreated>().Wait();

                Console.WriteLine("===== Billing =====");

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
