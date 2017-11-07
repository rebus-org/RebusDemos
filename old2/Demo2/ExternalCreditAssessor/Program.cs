using System;
using Common;
using Microsoft.Owin.Hosting;

// ReSharper disable ArgumentsStyleNamedExpression

namespace ExternalCreditAssessor
{
    class Program
    {
        static void Main()
        {
            using (WebApp.Start<Startup>(Config.CreditAssessmentUrl))
            {
                Console.WriteLine("External credit assessment service started...");
                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}