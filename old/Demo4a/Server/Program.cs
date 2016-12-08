using System;
using Rebus.Configuration;
using Rebus.Transports.Msmq;
using Server.Handlers;
using Rebus.Log4Net;
using log4net.Config;

namespace Server
{
    class Program
    {
        static void Main()
        {
            XmlConfigurator.Configure();

            using (var adapter = new BuiltinContainerAdapter())
            {
                adapter.Register(() => new RandomRequestHandler(adapter.Bus));

                Configure.With(adapter)
                         .Logging(l => l.Log4Net())
                         .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                         .CreateBus()
                         .Start();

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
