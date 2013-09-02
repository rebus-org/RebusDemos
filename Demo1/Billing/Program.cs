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
                         .Logging(l => l.None())
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

    class ChargeTheCustomer : IHandleMessages<NewTradeRecorded>
    {
        public void Handle(NewTradeRecorded message)
        {
            Console.WriteLine(@"New trade {0} recorded for {1}
    Amount: {2:0.0}
    Price: {3:0.00}
", message.Id, message.Counterpart, message.Amount, message.Price);
        }
    }
}
