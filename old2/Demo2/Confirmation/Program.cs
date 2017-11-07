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
            using (var creditAssessmentClient = new HttpClient())
            {
                creditAssessmentClient.BaseAddress = new Uri(Config.CreditAssessmentUrl);

                using (var activator = new BuiltinHandlerActivator())
                {
                    activator.Register((bus, context) => new ConfirmationHandler(bus, creditAssessmentClient));

                    Configure.With(activator)
                        .ConfigureEndpoint(Config.Queues.Confirmation)
                        .Options(o =>
                        {
                            o.SetMaxParallelism(100);
                        })
                        .Start();

                    activator.Bus.Subscribe<TradeCreated>().Wait();

                    Console.WriteLine("===== Confirmation =====");

                    Console.WriteLine("Press ENTER to quit");
                    Console.ReadLine();
                }
            }
        }
    }
}
