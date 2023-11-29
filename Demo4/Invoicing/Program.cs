using System;
using Confirmation.Messages;
using Invoicing.Handlers;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Trading.Messages;
// ReSharper disable ArgumentsStyleNamedExpression
// ReSharper disable ArgumentsStyleLiteral

namespace Invoicing
{
    class Program
    {
        const string ConnectionString = "server=.; database=RebusDemos; trusted_connection=true; encrypt=false";

        static void Main()
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Register((bus, context) => new InvoicingSaga(bus));

                Configure.With(activator)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Subscriptions(s => s.StoreInSqlServer(ConnectionString, "Subscriptions", isCentralized: true))
                    .Sagas(s => s.StoreInSqlServer(ConnectionString, "Sagas", "SagaIndex"))
                    .Timeouts(s => s.StoreInSqlServer(ConnectionString, "Timeouts"))
                    .Transport(t => t.UseMsmq("invoicing"))
                    .Start();

                activator.Bus.Subscribe<TradeRecorded>().Wait();
                activator.Bus.Subscribe<TradeApproved>().Wait();
                activator.Bus.Subscribe<TradeRejected>().Wait();

                Console.WriteLine("Invoicing is running - press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
