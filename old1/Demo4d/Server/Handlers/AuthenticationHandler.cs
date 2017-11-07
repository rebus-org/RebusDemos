using System;
using Rebus;
using Rebus.Shared;

namespace Server.Handlers
{
    public class AuthenticationHandler : IHandleMessages<object>
    {
        public void Handle(object message)
        {
            Console.WriteLine("Authenticating....");
            var context = MessageContext.GetCurrent();
            var headers = context.Headers;
            if (!headers.ContainsKey(Headers.UserName))
            {
                context.Abort();
                return;
            }
            var username = headers[Headers.UserName];
            Console.WriteLine("{0} authenticated!", username);
        }
    }
}