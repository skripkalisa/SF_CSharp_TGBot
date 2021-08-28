namespace Bot.Commands
{
    public abstract class ChatTextCommandOption : AbstractCommand
    {
        public override bool CheckMessage(string message)
        {
            return message.StartsWith(CommandText);
        }

        protected string ClearMessageFromCommand(string message)
        {
            return message.Substring(CommandText.Length + 1);
        }

    }
}
