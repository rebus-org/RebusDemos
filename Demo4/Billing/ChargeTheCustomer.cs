using System;
using Confirmations.Messages;
using Rebus;
using Trading.Messages;

namespace Billing
{
    class ChargeTheCustomer : Saga<BillingSagaData>, 
        IAmInitiatedBy<NewTradeRecorded>,
        IAmInitiatedBy<CounterpartConfirmed>,
        IAmInitiatedBy<CounterpartRejected>,
        IHandleMessages<VerifyComplete>
    {
        readonly IBus bus;

        public ChargeTheCustomer(IBus bus)
        {
            this.bus = bus;
        }

        public override void ConfigureHowToFindSaga()
        {
            Incoming<NewTradeRecorded>(m => m.Counterpart).CorrelatesWith(d => d.Counterpart);
            Incoming<CounterpartConfirmed>(m => m.Counterpart).CorrelatesWith(d => d.Counterpart);
            Incoming<CounterpartRejected>(m => m.Counterpart).CorrelatesWith(d => d.Counterpart);
            Incoming<VerifyComplete>(m => m.CorrelationId).CorrelatesWith(d => d.Id);
        }

        public void Handle(NewTradeRecorded message)
        {
            if (IsNew)
            {
                bus.Defer(TimeSpan.FromSeconds(10), new VerifyComplete {CorrelationId = Data.Id});
            }

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
            if (IsNew)
            {
                bus.Defer(TimeSpan.FromSeconds(10), new VerifyComplete { CorrelationId = Data.Id });
            }

            Data.Counterpart = message.Counterpart;

            Data.GotCreditStatus = true;
            Data.CreditOk = true;

            Console.WriteLine("Counterpart credit status {0} confirmed", message.Counterpart);

            PossiblyBillTheCustomer();
        }

        public void Handle(CounterpartRejected message)
        {
            if (IsNew)
            {
                bus.Defer(TimeSpan.FromSeconds(10), new VerifyComplete { CorrelationId = Data.Id });
            }

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
=========================", Data.Counterpart, Data.CreditOk ? "OK" : "NOT OK");
            }
            else
            {
                Console.WriteLine(@"=========================
    counterpart: {0}
    credit:      {1}
    
    u y no have moneys?!
=========================", Data.Counterpart, Data.CreditOk ? "OK" : "NOT OK");
            }
            Console.WriteLine(@"=========================");

            MarkAsComplete();
        }

        public void Handle(VerifyComplete message)
        {
            Console.WriteLine(
                @"!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
Oh noes!!!11

The saga for {0} was not completed within timeout! 

Now we probably want to send an email or something...
!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",
                Data.Counterpart);
        }
    }

    class VerifyComplete
    {
        public Guid CorrelationId { get; set; }
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