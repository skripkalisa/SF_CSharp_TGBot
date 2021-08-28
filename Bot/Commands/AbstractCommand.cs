namespace Bot.Commands
{
    public abstract class AbstractCommand : IChatCommand
    {
        protected string CommandText;

        public virtual bool CheckMessage(string message)
        {
            return CommandText == message;
        }
    }
}
