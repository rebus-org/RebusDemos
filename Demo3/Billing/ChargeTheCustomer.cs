using System;
using Confirmations.Messages;
using Rebus;
using Trading.Messages;

namespace Billing
{
    class ChargeTheCustomer : Saga<BillingSagaData>, 
        IAmInitiatedBy<NewTradeRecorded>,
        IAmInitiatedBy<TradeConfirmed>,
        IAmInitiatedBy<TradeRejected>,
        IHandleMessages<VerifyComplete>
    {
        readonly IBus bus;

        public ChargeTheCustomer(IBus bus)
        {
            this.bus = bus;
        }

        public override void ConfigureHowToFindSaga()
        {
            Incoming<NewTradeRecorded>(m => m.TradeId).CorrelatesWith(d => d.TradeId);
            Incoming<TradeConfirmed>(m => m.TradeId).CorrelatesWith(d => d.TradeId);
            Incoming<TradeRejected>(m => m.TradeId).CorrelatesWith(d => d.TradeId);
            Incoming<VerifyComplete>(m => m.CorrelationId).CorrelatesWith(d => d.Id);
        }

        public void Handle(NewTradeRecorded message)
        {
            PossiblyScheduleVerification();

            Data.TradeId = message.TradeId;
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

        public void Handle(TradeConfirmed message)
        {
            PossiblyScheduleVerification();

            Data.TradeId = message.TradeId;

            Data.GotCreditStatus = true;
            Data.CreditOk = true;

            Console.WriteLine("Counterpart credit status confirmed for trade {0}", message.TradeId);

            PossiblyBillTheCustomer();
        }

        public void Handle(TradeRejected message)
        {
            PossiblyScheduleVerification();

            Data.TradeId = message.TradeId;

            Data.GotCreditStatus = true;
            Data.CreditOk = false;

            Console.WriteLine("Counterpart credit status NOT confirmed for trade {0}", message.TradeId);

            PossiblyBillTheCustomer();
        }

        public void Handle(VerifyComplete message)
        {
            Console.WriteLine(
                @"!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
Oh noes!!!11

The saga for trade {0}/counterpart {1} was not completed within timeout! 

Now we probably want to send an email or something...
!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!",
                Data.TradeId, Data.Counterpart);
        }

        void PossiblyScheduleVerification()
        {
            if (IsNew)
            {
                bus.Defer(TimeSpan.FromSeconds(10), new VerifyComplete {CorrelationId = Data.Id});
            }
        }

        void PossiblyBillTheCustomer()
        {
            if (!Data.GotCreditStatus) return;
            if (!Data.GotTradeDetails) return;

            if (Data.CreditOk)
            {
                Console.WriteLine(@"=========================
    trade id:    {0}
    counterpart: {1}
    credit:      {2}
    
    will send invoice
    w. credit
=========================", Data.TradeId, Data.Counterpart, Data.CreditOk ? "OK" : "NOT OK");
            }
            else
            {
                Console.WriteLine(@"=========================
    trade id:    {0}
    counterpart: {1}
    credit:      {2}
    
    will send an invoice
    and ask him to pay
    immediately
=========================", Data.TradeId, Data.Counterpart, Data.CreditOk ? "OK" : "NOT OK");
            }

            MarkAsComplete();
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

        public Guid TradeId { get; set; }
        public string Counterpart { get; set; }

        public bool GotTradeDetails { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }

        public bool GotCreditStatus { get; set; }
        public bool CreditOk { get; set; }
    }
}