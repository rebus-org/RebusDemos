using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Owin;

namespace ExternalCreditAssessor
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/check-credit", CheckCredit);
        }

        static void CheckCredit(IAppBuilder app)
        {
            var random = new ThreadLocal<Random>(() => new Random(DateTime.Now.GetHashCode()));

            app.Run(async context =>
            {
                var request = context.Request;

                if (request.Method != "GET")
                {
                    await context.WriteStatus(HttpStatusCode.BadRequest, "Must be GET");
                    return;
                }

                var query = request.Query;
                var counterparty = query.Get("counterparty")?.ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(counterparty))
                {
                    await context.WriteStatus(HttpStatusCode.BadRequest, reason: "Must include 'counterparty' form parameter");
                    return;
                }

                await Task.Delay(TimeSpan.FromSeconds(1));

                // 50% risk that call fails
                var shouldFail = random.Value.Next(2) == 0;

                if (shouldFail)
                {
                    await context.WriteStatus(HttpStatusCode.InternalServerError, reason: "Server experienced something bad");
                    return;
                }

                // 20% risk that counterparty is not good

                var isGood = random.Value.Next(5) != 0;

                await context.WriteStatus(HttpStatusCode.OK, body: isGood);
            });
        }
    }
}