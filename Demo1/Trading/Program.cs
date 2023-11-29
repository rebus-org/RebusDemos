using System;
using System.Threading.Tasks;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Trading.Messages;
// ReSharper disable ArgumentsStyleNamedExpression

namespace Trading;

class Program
{
    static async Task Main()
    {
        using var activator = new BuiltinHandlerActivator();

        var bus = Configure.With(activator)
            .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
            .Subscriptions(s => s.StoreInSqlServer("server=.; database=RebusDemos; trusted_connection=true; encrypt=false", "Subscriptions", isCentralized: true))
            .Transport(t => t.UseMsmq("trading"))
            .Start();

        Console.WriteLine("Trading is running");

        while (true)
        {
            Console.WriteLine("Please enter new trade details");
            Console.Write(" commodity > ");
            var commodity = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(commodity)) break;

            Console.Write("  quantity > ");
            int quantity;
            while (!int.TryParse(Console.ReadLine(), out quantity)) ;

            await bus.Publish(new TradeRecorded(GenerateNewTradeId(), commodity, quantity));
        }
    }

    static string GenerateNewTradeId() => $"trade-{DateTime.Now:yyyyMMdd-HHmmss}";
}