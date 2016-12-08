using System;
using Common;
using Rebus.Activation;
using Rebus.Config;
using Trading.Messages;

// ReSharper disable ArgumentsStyleLiteral

namespace Trading
{
    class Program
    {
        static void Main()
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                var bus = Configure.With(activator)
                    .ConfigureEndpoint("trading")
                    .Start();

                Console.WriteLine("===== Trading =====");

                while (true)
                {
                    var counterparty = Prompt<string>("Counterparty");

                    if (string.IsNullOrWhiteSpace(counterparty)) break;

                    var commodity = Prompt<string>("Commodity");
                    var quantity = Prompt<decimal>("Quantity");

                    bus.Publish(new TradeCreated(IdGenerator.NewId("trade"), counterparty, commodity, quantity)).Wait();
                }
            }
        }

        static T Prompt<T>(string what)
        {
            Console.Write(what + " > ");
            var commodity = Console.ReadLine();
            return (T) Convert.ChangeType(commodity, typeof(T));
        }
    }
}
