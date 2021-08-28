namespace Bot.Commands
{
    public interface IChatCommand
    {
        bool CheckMessage(string message);
    }
}
