using System;
using Confirmations.Messages;
using CreditAssessment.Integration.Messages;
using Rebus;
using Trading.Messages;

namespace Confirmations
{
    public class CheckCreditStatus : IHandleMessages<NewTradeRecorded>, IHandleMessages<GetCreditStatusReply>
    {
        readonly IBus bus;

        public CheckCreditStatus(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(NewTradeRecorded message)
        {
            Console.WriteLine("Checking credit status for {0}", message.Counterpart);
            bus.Send(new GetCreditStatus
                         {
                             Counterpart = message.Counterpart,
                             CorrelationId = message.TradeId,
                         });
        }

        public void Handle(GetCreditStatusReply message)
        {
            var tradeId = message.CorrelationId;

            if (message.Ok)
            {
                Console.WriteLine("Publishing OK for trade {0}", tradeId);
                bus.Publish(new TradeConfirmed {TradeId = tradeId});
            }
            else
            {
                Console.WriteLine("Publishing NOT OK for trade {0}", tradeId);
                bus.Publish(new TradeRejected {TradeId = tradeId});
            }
        }
    }
}