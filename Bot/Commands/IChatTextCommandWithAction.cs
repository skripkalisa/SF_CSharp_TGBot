namespace Bot.Commands
{
    interface IChatTextCommandWithAction: IChatTextCommand
    {
        bool DoAction(Conversation chat);
    }
}
