using System;
using Rebus;

namespace Server.Handlers
{
    public class GreetingHandler : IHandleMessages<string>
    {
        readonly IBus bus;

        public GreetingHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(string message)
        {
            Console.WriteLine("Sending reply to greeting containing {0} chars", message.Length);

            bus.Reply(string.Format("Thank you for your message containing {0} chars", message.Length));
        }
    }
}