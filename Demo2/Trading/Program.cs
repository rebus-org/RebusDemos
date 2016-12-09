using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    .ConfigureEndpoint(Config.Queues.Trading)
                    .Start();

                Console.WriteLine("===== Trading =====");

                while (true)
                {
                    Console.WriteLine("Enter (s)ingle trade, or auto-generate (m)ultiple?");
                    var key = ReadKey("sm");

                    if (key == 's')
                    {
                        var counterparty = Prompt<string>("Counterparty");

                        if (string.IsNullOrWhiteSpace(counterparty)) break;

                        var commodity = Prompt<string>("Commodity");
                        var quantity = Prompt<decimal>("Quantity");

                        bus.Publish(new TradeCreated(IdGenerator.NewId("trade"), counterparty, commodity, quantity)).Wait();
                    }
                    else
                    {
                        var events = Enumerable.Range(0, 10)
                            .Select(index => new TradeCreated(IdGenerator.NewId("trade"),
                                $"Counterparty {index + 1}", "Oil", (index + 1) * 10));

                        Task.WaitAll(events.Select(e => bus.Publish(e)).ToArray());
                    }
                }
            }
        }

        static char ReadKey(IEnumerable<char> allowedChars)
        {
            var allowedCharsHash = new HashSet<char>(allowedChars.Select(char.ToLowerInvariant));
            while (true)
            {
                var keyChar = char.ToLowerInvariant(Console.ReadKey(true).KeyChar);
                if (!allowedCharsHash.Contains(keyChar)) continue;
                return keyChar;
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
