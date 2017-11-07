using System;
using CreditAssessment.Integration.Credit;
using CreditAssessment.Integration.Messages;
using Rebus;

namespace CreditAssessment.Integration
{
    public class GetCreditStatusHandler : IHandleMessages<GetCreditStatus>
    {
        readonly IBus bus;

        public GetCreditStatusHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(GetCreditStatus message)
        {
            using (var client = new CreditAssessmentClient())
            {
                var isOk = client.IsOk(message.Counterpart);

                Console.WriteLine("Counterpart {0} status: {1}", message.Counterpart, isOk ? "OK" : "NOT OK!!");

                bus.Reply(new GetCreditStatusReply
                              {
                                  CorrelationId = message.CorrelationId,
                                  Ok = isOk
                              });
            }
        }
    }
}