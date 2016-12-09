using Rebus.Config;
using Rebus.Logging;

namespace Common
{
    public static class RebusConfigurationExtensions
    {
        public static RebusConfigurer ConfigureEndpoint(this RebusConfigurer configurer, string inputQueueName)
        {
            return configurer
                .Logging(l => l.ColoredConsole(LogLevel.Warn))
                .Transport(t => t.UseMsmq(inputQueueName))
                .Subscriptions(s => s.StoreInSqlServer(Config.ConnectionString, "Subscriptions", isCentralized: true))
                ;
        }
    }
}