namespace Common
{
    public static class Config
    {
        public const string ConnectionString = "server=.; database=rebusdemos; trusted_connection=true";
        public const string CreditAssessmentUrl = "http://localhost:7000";

        public class Queues
        {
            public const string Trading = "trading";
            public const string Billing = "billing";
            public const string Confirmation = "confirmation";
        }
    }
}
