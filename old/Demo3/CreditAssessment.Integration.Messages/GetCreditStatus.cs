using System;

namespace CreditAssessment.Integration.Messages
{
    public class GetCreditStatus
    {
        public Guid CorrelationId { get; set; }
        public string Counterpart { get; set; }
    }
}
