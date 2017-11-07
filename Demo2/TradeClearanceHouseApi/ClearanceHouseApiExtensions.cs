using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using TradeClearanceHouseApi.Dtos;

namespace TradeClearanceHouseApi
{
    public static class ClearanceHouseApiExtensions
    {
        public static void RunClearanceHouseApi(this IAppBuilder app)
        {
            var random = new ThreadLocal<Random>(() => new Random(DateTime.Now.GetHashCode()));

            app.MapWhen(c => c.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase), a =>
            {
                a.Run(async context =>
                {
                    var request = await Deserialize<CheckTradeRequest>(context.Request);

                    Console.Write($"Clearing trade {request.TradeId} ({request.Quantity} x {request.Commodity})...");

                    await Task.Delay(1000);

                    var ok = random.Value.NextDouble() < 0.5;

                    Console.WriteLine($" result: {(ok ? "OK" : "Not ok")}");

                    await context.Response.WriteAsync(Serialize(new CheckTradeResponse(ok)));
                });
            });
        }

        static string Serialize(object obj) => JsonConvert.SerializeObject(obj);

        static async Task<T> Deserialize<T>(IOwinRequest request)
        {
            try
            {
                using (var reader = new StreamReader(request.Body, Encoding.UTF8))
                {
                    var json = await reader.ReadToEndAsync();

                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}