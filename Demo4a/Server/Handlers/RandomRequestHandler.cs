using System.Reflection;
using Rebus;
using Server.Messages;
using log4net;

namespace Server.Handlers
{
    public class RandomRequestHandler : IHandleMessages<SomeRandomRequest>
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IBus bus;

        public RandomRequestHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(SomeRandomRequest request)
        {
            Log.InfoFormat("Got request");
            Log.InfoFormat("Will send reply now");
            Log.InfoFormat("After having done some serious logging thoughout the handling of the request");
            
            bus.Reply(new SomeRandomReply());
        }
    }
}