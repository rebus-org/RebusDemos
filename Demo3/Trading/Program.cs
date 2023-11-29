using System;
using System.Linq;
using System.Threading.Tasks;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Trading.Messages;
// ReSharper disable ArgumentsStyleNamedExpression
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
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Subscriptions(s => s.StoreInSqlServer("server=.; database=RebusDemos; trusted_connection=true; encrypt=false", "Subscriptions", isCentralized: true))
                    .Transport(t => t.UseMsmq("trading"))
                    .Start();

                Console.WriteLine("Trading is running");

                while (true)
                {
                    Console.WriteLine("(1) single trade, (2) many trades, (q) quit");

                    var key = Console.ReadKey(true);

                    if (key.KeyChar == '1')
                    {
                        Console.WriteLine("Please enter new trade details");
                        Console.Write(" commodity > ");
                        var commodity = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(commodity)) break;

                        Console.Write("  quantity > ");
                        int quantity;
                        while (!int.TryParse(Console.ReadLine(), out quantity)) ;

                        bus.Publish(new TradeRecorded(GenerateNewTradeId(), commodity, quantity)).Wait();
                    }
                    else if (key.KeyChar == '2')
                    {
                        var tradeId = GenerateNewTradeId();

                        Task.WaitAll(Enumerable.Range(0, 30)
                            .Select(i => bus.Publish(new TradeRecorded($"{tradeId}/{i}", "Cowboytoast", i*7%11)))
                            .ToArray());
                    }
                }
            }

        }

        static string GenerateNewTradeId() => $"trade-{DateTime.Now:yyyyMMdd-HHmmss}";
    }
}
