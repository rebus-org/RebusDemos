using System;
using Client.Handlers;
using Rebus.Configuration;
using Rebus.Transports.Msmq;
using Server.Messages;
using Rebus.Log4Net;
using log4net.Config;

namespace Client
{
    class Program
    {
        static void Main()
        {
            XmlConfigurator.Configure();

            using (var adapter = new BuiltinContainerAdapter())
            {
                adapter.Register(typeof (RandomReplyHandler));

                Configure.With(adapter)
                         .Logging(l => l.Log4Net())
                         .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                         .MessageOwnership(o => o.FromRebusConfigurationSection())
                         .CreateBus()
                         .Start();

                var bus = adapter.Bus;

                while (true)
                {
                    Console.WriteLine("Press (a) to send a request and (q) to quit");

                    var c = Console.ReadKey(true).KeyChar;

                    if (c == 'q') break;

                    var request = new SomeRandomRequest();

                    bus.Send(request);
                }
            }
        }
    }
}
