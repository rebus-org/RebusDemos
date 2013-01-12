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
            bus.Send(new GetCreditStatus {Counterpart = message.Counterpart});
        }

        public void Handle(GetCreditStatusReply message)
        {
            if (message.Ok)
            {
                Console.WriteLine("Publishing OK for {0}", message.Counterpart);
                bus.Publish(new CounterpartConfirmed
                                {
                                    Counterpart = message.Counterpart
                                });
            }
            else
            {
                Console.WriteLine("Publishing NOT OK for {0}", message.Counterpart);
                bus.Publish(new CounterpartRejected
                                {
                                    Counterpart = message.Counterpart
                                });
            }
        }
    }
}