using System;
using System.Threading.Tasks;
using Invoicing.Handlers;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Trading.Messages;
// ReSharper disable ArgumentsStyleNamedExpression
// ReSharper disable ArgumentsStyleLiteral

namespace Invoicing;

class Program
{
    static async Task Main()
    {
        using var activator = new BuiltinHandlerActivator();
        
        activator.Register(() => new TradeRecordedHandler());

        var bus = Configure.With(activator)
            .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
            .Subscriptions(s => s.StoreInSqlServer("server=.; database=RebusDemos; trusted_connection=true; encrypt=false", "Subscriptions", isCentralized: true))
            .Transport(t => t.UseMsmq("invoicing"))
            .Start();

        await bus.Subscribe<TradeRecorded>();

        Console.WriteLine("Invoicing is running - press ENTER to quit");
        Console.ReadLine();
    }
}