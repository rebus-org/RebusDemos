using System;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Routing.TypeBased;

namespace Client
{
    class Program
    {
        static void Main()
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                var bus = Configure.With(activator)
                    .Logging(l => l.ColoredConsole(LogLevel.Warn))
                    .Transport(t => t.UseMsmqAsOneWayClient())
                    .Routing(t => t.TypeBased().Map<string>("server"))
                    .Start();

                while (true)
                {
                    Console.Write("> ");
                    var text = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(text)) break;

                    bus.Send(text);
                }
            }
        }
    }
}
