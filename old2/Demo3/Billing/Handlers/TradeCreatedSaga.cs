using System;
using System.Threading.Tasks;
using Billing.Messages;
using Confirmation.Messages;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Sagas;
using Trading.Messages;

namespace Billing.Handlers
{
    public class TradeCreatedSaga : Saga<TradeCreatedSaga.TradeCreatedSagaData>,
        IAmInitiatedBy<TradeCreated>,
        IAmInitiatedBy<TradeAccepted>, 
        IAmInitiatedBy<TradeRejected>,
        IHandleMessages<TradeSagaTimeout>
    {
        readonly IBus _bus;

        public TradeCreatedSaga(IBus bus)
        {
            _bus = bus;
        }

        protected override void CorrelateMessages(ICorrelationConfig<TradeCreatedSagaData> config)
        {
            // events
            config.Correlate<TradeCreated>(message => message.TradeId, data => data.TradeId);
            config.Correlate<TradeAccepted>(message => message.TradeId, data => data.TradeId);
            config.Correlate<TradeRejected>(message => message.TradeId, data => data.TradeId);

            // timeout
            config.Correlate<TradeSagaTimeout>(message => message.TradeId, data => data.TradeId);
        }

        public class TradeCreatedSagaData : SagaData
        {
            public string TradeId { get; set; }
            public string Counterparty { get; set; }

            public bool TradeWasAccepted { get; set; }
            public bool TradeWasRejected { get; set; }
        }

        public async Task Handle(TradeCreated message)
        {
            Console.WriteLine($"New trade created: {message.TradeId}");

            await MaybeOrderTimeout();

            Data.Counterparty = message.Counterparty;

            await MaybeCompleteSaga();
        }

        public async Task Handle(TradeAccepted message)
        {
            Console.WriteLine($"Trade accepted: {message.TradeId}");

            await MaybeOrderTimeout();

            Data.TradeWasAccepted = true;

            await MaybeCompleteSaga();
        }

        public async Task Handle(TradeRejected message)
        {
            Console.WriteLine($"Trade rejected: {message.TradeId}");

            await MaybeOrderTimeout();

            Data.TradeWasRejected = true;

            await MaybeCompleteSaga();
        }

        public async Task Handle(TradeSagaTimeout message)
        {
            Console.WriteLine($"Trade timeout elapsed: {message.TradeId}");

            await _bus.SendLocal(new TakeOutForManualInspection(Data.TradeId));

            MarkAsComplete();
        }

        async Task MaybeOrderTimeout()
        {
            if (!IsNew) return;

            await _bus.Defer(TimeSpan.FromSeconds(10), new TradeSagaTimeout(Data.TradeId));
        }

        async Task MaybeCompleteSaga()
        {
            if (Data.TradeWasRejected)
            {
                Console.WriteLine($"Trade {Data.TradeId} rejected... not doing anything");
                MarkAsComplete();
                return;
            }

            if (Data.TradeWasAccepted && Data.Counterparty != null)
            {
                await _bus.SendLocal(new SendInvoice(Data.Counterparty, Data.TradeId));

                MarkAsComplete();
            }
        }
    }
}