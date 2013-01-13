using System;

namespace Trading.Messages
{
    public class NewTradeRecorded
    {
        public Guid Id { get; set; }
        public string Counterpart { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
    }
}
