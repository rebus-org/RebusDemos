using System;
using Confirmations.Messages;
using Rebus.Configuration;
using Rebus.Logging;
using Rebus.Transports.Msmq;
using Trading.Messages;
using Rebus.MongoDb;

namespace Billing
{
    class Program
    {
        static void Main()
        {
            using (var adapter = new BuiltinContainerAdapter())
            {
                adapter.Register(() => new ChargeTheCustomer(adapter.Bus));

                Configure.With(adapter)
                         .Logging(l => l.ColoredConsole(LogLevel.Error))
                         .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                         .MessageOwnership(o => o.FromRebusConfigurationSection())
                         .Sagas(s => s.StoreInMongoDb("mongodb://localhost/billing")
                                      .SetCollectionName<BillingSagaData>("billingSagas"))
                         .Timeouts(t => t.StoreInMongoDb("mongodb://localhost/billing", "timeouts"))
                         .CreateBus()
                         .Start();

                Console.WriteLine("----Billing----");

                adapter.Bus.Subscribe<NewTradeRecorded>();
                adapter.Bus.Subscribe<CounterpartConfirmed>();
                adapter.Bus.Subscribe<CounterpartRejected>();

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
