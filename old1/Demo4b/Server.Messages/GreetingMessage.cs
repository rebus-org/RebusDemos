namespace Server.Messages
{
    public class GreetingMessage
    {
        public GreetingMessage(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }
}
