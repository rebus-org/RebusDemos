using System;
using Rebus.Configuration;
using Rebus.Logging;
using Rebus.Transports.Msmq;
using Server.Handlers;

namespace Server
{
    class Program
    {
        static void Main()
        {
            using (var adapter = new BuiltinContainerAdapter())
            {
                adapter.Register(() => new GreetingHandler(adapter.Bus));

                Configure.With(adapter)
                         .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                         .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                         .CreateBus()
                         .Start();

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
