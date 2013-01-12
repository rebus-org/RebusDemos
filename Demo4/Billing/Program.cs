using System;
using Rebus.Configuration;
using Rebus.Transports.Msmq;
using Trading.Messages;
using Rebus.Logging;

namespace Billing
{
    class Program
    {
        static void Main()
        {
            using (var adapter = new BuiltinContainerAdapter())
            {
                adapter.Register(typeof (BillTheCustomer));

                Configure.With(adapter)
                         .Logging(l => l.ColoredConsole(LogLevel.Error))
                         .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                         .MessageOwnership(o => o.FromRebusConfigurationSection())
                         .CreateBus()
                         .Start();

                Console.WriteLine("----Billing----");

                adapter.Bus.Subscribe<NewTradeRecorded>();

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
