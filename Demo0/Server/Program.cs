using System;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
// ReSharper disable ArgumentsStyleNamedExpression
#pragma warning disable 1998

namespace Server;

class Program
{
    static void Main()
    {
        using var activator = new BuiltinHandlerActivator();
        
        activator.Handle<string>(async str =>
        {
            Console.WriteLine($"Got greeting: {str}");
        });

        Configure.With(activator)
            .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
            .Transport(t => t.UseMsmq("server"))
            .Start();

        Console.WriteLine("Press ENTER to quit");
        Console.ReadLine();
    }
}