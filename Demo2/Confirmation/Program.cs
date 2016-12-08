using System;
using System.Net.Http;
using Common;
using Confirmation.Handlers;
using Rebus.Activation;
using Rebus.Config;
using Trading.Messages;

// ReSharper disable AccessToDisposedClosure

namespace Confirmation
{
    class Program
    {
        static void Main()
        {
            using (var httpClient = new HttpClient())
            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Register((bus, context) => new ConfirmationHandler(bus, httpClient));

                Configure.With(activator)
                    .ConfigureEndpoint("confirmation")
                    .Start();

                activator.Bus.Subscribe<TradeCreated>().Wait();

                Console.WriteLine("===== Confirmation =====");

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
