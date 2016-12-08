using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using Microsoft.Owin.Hosting;
using Owin;

namespace ExternalCreditAssessor
{
    class Program
    {
        static void Main()
        {
            using (WebApp.Start<Startup>("https://localhost:7000"))
            {
                Console.WriteLine("External credit assessment service started...");
                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var checks = new ConcurrentDictionary<string, int>();
            var getRandom = new ThreadLocal<Random>(() => new Random(DateTime.Now.GetHashCode()));

            app.Map("/check-credit", a =>
            {
                a.Run(async context =>
                {
                    var request = context.Request;

                    if (request.Method != "GET")
                    {
                        await context.WriteStatus(HttpStatusCode.BadRequest, "Must be GET");
                    }

                    var query = request.Query;
                    var counterparty = query.Get("counterparty")?.ToLowerInvariant();

                    if (string.IsNullOrWhiteSpace(counterparty))
                    {
                        await context.WriteStatus(HttpStatusCode.BadRequest, "Must include 'counterparty' form parameter");
                        return;
                    }

                    var numberOfChecks = checks.AddOrUpdate(counterparty, 1, (c, n) => n + 1);
                    var random = getRandom.Value.Next(numberOfChecks);


                });
            });
        }
    }
}

