using System;
using Rebus.Configuration;
using Rebus.Logging;
using Rebus.Transports.Msmq;

namespace CreditAssessment.Integration
{
    class Program
    {
        static void Main()
        {
            using (var adapter = new BuiltinContainerAdapter())
            {
                adapter.Register(() => new GetCreditStatusHandler(adapter.Bus));

                Configure.With(adapter)
                         .Logging(l => l.ColoredConsole(LogLevel.Error))
                         .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                         .CreateBus()
                         .Start();

                Console.WriteLine("----Credit.Integration----");
                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
