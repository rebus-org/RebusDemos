using System;

namespace CreditAssessment.Integration.Messages
{
    public class GetCreditStatusReply
    {
        public Guid CorrelationId { get; set; }
        public bool Ok { get; set; }
    }
}