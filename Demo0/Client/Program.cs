using System;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Routing.TypeBased;
// ReSharper disable ArgumentsStyleNamedExpression

namespace Client
{
    class Program
    {
        static void Main()
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                var bus = Configure.With(activator)
                    .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                    .Transport(t => t.UseMsmq("client"))
                    .Routing(r => r.TypeBased().Map<string>("server"))
                    .Start();

                while (true)
                {
                    Console.Write("Type greeting > ");
                    var text = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(text)) break;

                    bus.Send(text).Wait();
                }
            }

        }
    }
}
