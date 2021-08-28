namespace Bot.Commands
{
    public class StopTrainingCommand : AbstractCommand, IChatTextCommandWithAction
    {
        public StopTrainingCommand()
        {
            CommandText = "/stop";
        }

        public bool DoAction(Conversation chat)
        {
            chat.IsTrainingInProcess = false;
            return !chat.IsTrainingInProcess;
        }

        public string ReturnText()
        {
            return "Тренировка остановлена!";
        }
    }
}
