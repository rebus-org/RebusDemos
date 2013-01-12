using System;
using Confirmations.Messages;
using Rebus;
using Trading.Messages;

namespace Billing
{
    class BillTheCustomer : Saga<BillingSagaData>, 
        IAmInitiatedBy<NewTradeRecorded>,
        IAmInitiatedBy<CounterpartConfirmed>,
        IAmInitiatedBy<CounterpartRejected>
    {
        public override void ConfigureHowToFindSaga()
        {
            Incoming<NewTradeRecorded>(m => m.Counterpart).CorrelatesWith(d => d.Counterpart);
            Incoming<CounterpartConfirmed>(m => m.Counterpart).CorrelatesWith(d => d.Counterpart);
            Incoming<CounterpartRejected>(m => m.Counterpart).CorrelatesWith(d => d.Counterpart);
        }

        public void Handle(NewTradeRecorded message)
        {
            Data.Counterpart = message.Counterpart;

            Data.Amount = message.Amount;
            Data.Price = message.Price;
            Data.GotTradeDetails = true;

            Console.WriteLine(@"New trade recorded for {0}
    Amount: {1:0.0}
    Price: {2:0.00}
", message.Counterpart, message.Amount, message.Price);

            PossiblyBillTheCustomer();
        }

        public void Handle(CounterpartConfirmed message)
        {
            Data.Counterpart = message.Counterpart;

            Data.GotCreditStatus = true;
            Data.CreditOk = true;

            Console.WriteLine("Counterpart credit status {0} confirmed", message.Counterpart);

            PossiblyBillTheCustomer();
        }

        public void Handle(CounterpartRejected message)
        {
            Data.Counterpart = message.Counterpart;

            Data.GotCreditStatus = true;
            Data.CreditOk = false;

            Console.WriteLine("Counterpart credit status {0} NOT confirmed", message.Counterpart);

            PossiblyBillTheCustomer();
        }

        void PossiblyBillTheCustomer()
        {
            if (!Data.GotCreditStatus) return;
            if (!Data.GotTradeDetails) return;

            if (Data.CreditOk)
            {
                Console.WriteLine(@"=========================
    counterpart: {0}
    credit:      {1}
    
    sending bill!!
=========================", Data.Counterpart);
            }
            else
            {
                Console.WriteLine(@"=========================
    counterpart: {0}
    credit:      {1}
    
    u y no have moneys?!
=========================", Data.Counterpart);
            }
            Console.WriteLine(@"=========================");

            MarkAsComplete();
        }
    }

    class BillingSagaData : ISagaData
    {
        public Guid Id { get; set; }
        public int Revision { get; set; }

        public string Counterpart { get; set; }

        public bool GotTradeDetails { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }

        public bool GotCreditStatus { get; set; }
        public bool CreditOk { get; set; }
    }
}