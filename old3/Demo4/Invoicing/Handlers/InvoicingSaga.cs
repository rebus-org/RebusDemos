using System;
using System.Threading.Tasks;
using Confirmation.Messages;
using Invoicing.Messages;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Trading.Messages;

namespace Invoicing.Handlers
{
    public class InvoicingSaga : Saga<InvoicingSagaData>,
        IAmInitiatedBy<TradeRecorded>,
        IAmInitiatedBy<TradeApproved>,
        IAmInitiatedBy<TradeRejected>,
        IHandleMessages<VerifyComplete>
    {
        readonly IBus _bus;

        public InvoicingSaga(IBus bus)
        {
            _bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<InvoicingSagaData> config)
        {
            config.Correlate<TradeRecorded>(m => m.TradeId, d => d.TradeId);
            config.Correlate<TradeApproved>(m => m.TradeId, d => d.TradeId);
            config.Correlate<TradeRejected>(m => m.TradeId, d => d.TradeId);
            config.Correlate<VerifyComplete>(m => m.TradeId, d => d.TradeId);
        }

        public async Task Handle(TradeRecorded message)
        {
            if (IsNew)
            {
                await _bus.DeferLocal(TimeSpan.FromSeconds(10), new VerifyComplete(message.TradeId));
            }

            Data.TradeId = message.TradeId;
            Data.Commodity = message.Commodity;
            Data.Quantity = message.Quantity;

            Console.WriteLine($@"Received trade details: {message.TradeId} ({message.Quantity} x {message.Commodity})");

            if (!Data.Confirmed.HasValue) return;

            if (Data.Confirmed.Value)
            {
                Console.WriteLine("The trade was already confirmed - adding to invoice now!");
                MarkAsComplete();
            }
            else
            {
                Console.WriteLine("The trade was already rejected - ignoring it.....");
                MarkAsComplete();
            }
        }

        public async Task Handle(TradeApproved message)
        {
            if (IsNew)
            {
                await _bus.DeferLocal(TimeSpan.FromSeconds(10), new VerifyComplete(message.TradeId));
            }

            Data.Confirmed = true;

            if (Data.HasTradeDetails())
            {
                Console.WriteLine($@"Trade details confirmed: {Data.TradeId} ({Data.Quantity} x {Data.Commodity}) - adding to invoice!");
                MarkAsComplete();
            }
        }

        public async Task Handle(TradeRejected message)
        {
            if (IsNew)
            {
                await _bus.DeferLocal(TimeSpan.FromSeconds(10), new VerifyComplete(message.TradeId));
            }

            Data.Confirmed = false;

            if (Data.HasTradeDetails())
            {
                Console.WriteLine($@"Trade details NOT confirmed: {Data.TradeId} ({Data.Quantity} x {Data.Commodity}) - ignoring the trade for now...");
                MarkAsComplete();
            }
        }

        public async Task Handle(VerifyComplete message)
        {
            // if we end up here, the process is still alive - that must mean that something is missing
            if (Data.Confirmed == null)
            {
                Console.WriteLine($"Missing confirmation status for trade {Data.TradeId} - we should probably handle this manually now");
            }

            if (!Data.HasTradeDetails())
            {
                Console.WriteLine($"Missing the trade details for trade {Data.TradeId} - we should probably handle this manually now");
            }

            MarkAsComplete();
        }
    }
}