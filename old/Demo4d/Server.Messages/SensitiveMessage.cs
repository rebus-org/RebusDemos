namespace Server.Messages
{
    public class SensitiveMessage
    {
        public SensitiveMessage(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }
}
