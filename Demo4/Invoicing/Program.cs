using System;
using System.Threading.Tasks;
using Confirmation.Messages;
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
    const string ConnectionString = "server=.; database=RebusDemos; trusted_connection=true; encrypt=false";

    static async Task Main()
    {
        using var activator = new BuiltinHandlerActivator();
        
        activator.Register((bus, context) => new InvoicingSaga(bus));

        Configure.With(activator)
            .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
            .Subscriptions(s => s.StoreInSqlServer(ConnectionString, "Subscriptions", isCentralized: true))
            .Sagas(s => s.StoreInSqlServer(ConnectionString, "Sagas", "SagaIndex"))
            .Timeouts(s => s.StoreInSqlServer(ConnectionString, "Timeouts"))
            .Transport(t => t.UseMsmq("invoicing"))
            .Start();

        await activator.Bus.Subscribe<TradeRecorded>();
        await activator.Bus.Subscribe<TradeApproved>();
        await activator.Bus.Subscribe<TradeRejected>();

        Console.WriteLine("Invoicing is running - press ENTER to quit");
        Console.ReadLine();
    }
}