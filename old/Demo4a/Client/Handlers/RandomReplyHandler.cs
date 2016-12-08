using System.Reflection;
using Rebus;
using Server.Messages;
using log4net;

namespace Client.Handlers
{
    public class RandomReplyHandler : IHandleMessages<SomeRandomReply>
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Handle(SomeRandomReply message)
        {
            Log.InfoFormat("Handling reply");
            Log.InfoFormat("By doing stuff");
            Log.InfoFormat("And possibly by sending messages somewhere else");
        }
    }
}