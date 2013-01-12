using System;
using Rebus.Configuration;
using Rebus.Transports.Msmq;
using Rebus.MongoDb;
using Trading.Messages;

namespace Trading
{
    class Program
    {
        static void Main()
        {
            using (var adapter = new BuiltinContainerAdapter())
            {
                Configure.With(adapter)
                         .Transport(t => t.UseMsmqAndGetInputQueueNameFromAppConfig())
                         .Subscriptions(s => s.StoreInMongoDb("mongodb://localhost/warmcroc", "tradingSubs"))
                         .CreateBus()
                         .Start();

                while (true)
                {
                    var counterpart = Input<string>("counterpart");
                    var amount = Input<decimal>("amount");
                    var price = Input<decimal>("price");

                    adapter.Bus.Publish(new NewTradeRecorded
                                            {
                                                Amount = amount,
                                                Counterpart = counterpart,
                                                Price = price
                                            });
                }
            }
        }

        static T Input<T>(string prompt)
        {
            Console.Write("{0}> ", prompt);
            var text = Console.ReadLine();
            return (T) Convert.ChangeType(text, typeof (T));
        }
    }
}
