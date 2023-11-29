using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Confirmation.Messages;
using Newtonsoft.Json;
using Rebus.Bus;
using Rebus.Handlers;
using Trading.Messages;

namespace Confirmation.Handlers;

public class TradeRecordedHandler : IHandleMessages<TradeRecorded>
{
    readonly IBus _bus;
    readonly HttpClient _http;

    public TradeRecordedHandler(IBus bus, HttpClient http)
    {
        _bus = bus;
        _http = http;
    }

    public async Task Handle(TradeRecorded message)
    {
        Console.Write($"Clearing trade {message.TradeId} ({message.Quantity} x {message.Commodity})... ");

        var response = await _http.PostAsync("http://localhost:30001", GetContent(message));

        response.EnsureSuccessStatusCode();

        var result = await Deserialize<ClearingResult>(response);
        var confirmed = result.Ok;

        if (confirmed)
        {
            Console.WriteLine("APPROVED");
            await _bus.Publish(new TradeApproved(message.TradeId));
        }
        else
        {
            Console.WriteLine("REJECTED");
            await _bus.Publish(new TradeRejected(message.TradeId));
        }
    }

    class ClearingResult
    {
        public bool Ok { get; }

        public ClearingResult(bool ok)
        {
            Ok = ok;
        }
    }

    static async Task<T> Deserialize<T>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception exception)
        {
            throw new FormatException($"Could not deserialize JSON text '{json}'", exception);
        }
    }

    static StringContent GetContent(TradeRecorded message)
    {
        var request = new
        {
            message.TradeId,
            message.Quantity,
            message.Commodity
        };
        var json = JsonConvert.SerializeObject(request);
        return new StringContent(json, Encoding.UTF8);
    }
}