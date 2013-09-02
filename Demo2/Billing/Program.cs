using System;
using Rebus;
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
                adapter.Register(typeof (ChargeTheCustomer));

                Configure.With(adapter)
                         .Logging(l => l.ColoredConsole(LogLevel.Error))
                         .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                         .MessageOwnership(o => o.FromRebusConfigurationSection())
                         .CreateBus()
                         .Start();

                Console.WriteLine("----Billing----");

                adapter.Bus.Subscribe<NewTradeRecorded>();

                Console.ReadLine();
            }
        }
    }

    class ChargeTheCustomer : IHandleMessages<NewTradeRecorded>
    {
        public void Handle(NewTradeRecorded message)
        {
            Console.WriteLine(@"New trade recorded for {0}
    Amount: {1:0.0}
    Price: {2:0.00}
", message.Counterpart, message.Amount, message.Price);
        }
    }
}
