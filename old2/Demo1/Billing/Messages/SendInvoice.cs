namespace Billing.Messages
{
    public class SendInvoice
    {
        public string Counterparty { get; }

        public SendInvoice(string counterparty)
        {
            Counterparty = counterparty;
        }
    }
}