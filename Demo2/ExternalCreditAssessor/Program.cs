using System;
using Microsoft.Owin.Hosting;

// ReSharper disable ArgumentsStyleNamedExpression

namespace ExternalCreditAssessor
{
    class Program
    {
        static void Main()
        {
            using (WebApp.Start<Startup>("http://localhost:7000"))
            {
                Console.WriteLine("External credit assessment service started...");
                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}