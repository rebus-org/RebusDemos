using System;
using Common;
using Rebus.Activation;
using Rebus.Config;

namespace Confirmation
{
    class Program
    {
        static void Main()
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                Configure.With(activator)
                    .ConfigureEndpoint("confirmation")
                    .Start();

                Console.WriteLine("===== Confirmation =====");

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
