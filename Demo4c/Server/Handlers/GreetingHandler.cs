using System;
using Rebus;
using Server.Messages;

namespace Server.Handlers
{
    public class GreetingHandler : IHandleMessages<GreetingMessage>
    {
        readonly IBus bus;

        public GreetingHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(GreetingMessage message)
        {
            Console.WriteLine("Sending reply to greeting containing {0} chars", message.Text.Length);

            bus.Reply(string.Format("Thank you for your message containing {0} chars", message.Text.Length));
        }
    }
}