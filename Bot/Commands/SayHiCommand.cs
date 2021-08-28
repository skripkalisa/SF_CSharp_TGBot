namespace Bot.Commands
{
    public class SayHiCommand : AbstractCommand, IChatTextCommand
    {
        public SayHiCommand()
        {
            CommandText = "/sayhi";
        }

        public string ReturnText()
        {
            return "Привет";
        }

    }
}
