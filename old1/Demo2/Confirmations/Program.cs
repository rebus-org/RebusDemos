using System;
using Rebus.Configuration;
using Rebus.Logging;
using Rebus.Transports.Msmq;
using Trading.Messages;

namespace Confirmations
{
    class Program
    {
        static void Main()
        {
            using (var adapter = new BuiltinContainerAdapter())
            {
                adapter.Register(() => new CheckCreditStatus(adapter.Bus));

                Configure.With(adapter)
                         .Logging(l => l.ColoredConsole(LogLevel.Error))
                         .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                         .MessageOwnership(o => o.FromRebusConfigurationSection())
                         .CreateBus()
                         .Start();

                Console.WriteLine("----Confirmations----");

                adapter.Bus.Subscribe<NewTradeRecorded>();

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
