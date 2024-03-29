﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Confirmation.Handlers;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Trading.Messages;
// ReSharper disable ArgumentsStyleNamedExpression
// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable AccessToDisposedClosure

namespace Confirmation
{
    class Program
    {
        static async Task Main()
        {
            using var activator = new BuiltinHandlerActivator();
            
            using var http = new HttpClient();
            
            activator.Register((bus, context) => new TradeRecordedHandler(bus, http));

            Configure.With(activator)
                .Logging(l => l.ColoredConsole(minLevel: LogLevel.Warn))
                .Subscriptions(s => s.StoreInSqlServer("server=.; database=RebusDemos; trusted_connection=true; encrypt=false", "Subscriptions", isCentralized: true))
                .Transport(t => t.UseMsmq("confirmation"))
                .Start();

            await activator.Bus.Subscribe<TradeRecorded>();

            Console.WriteLine("Confirmation is running - press ENTER to quit");
            Console.ReadLine();
        }
    }
}
