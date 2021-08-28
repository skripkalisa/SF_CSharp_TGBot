namespace Bot.Commands
{
    public class DeleteWordCommand : ChatTextCommandOption, IChatTextCommandWithAction
    {
        public DeleteWordCommand()
        {
            CommandText = "/delword";
        }

        public bool DoAction(Conversation chat)
        {
            var message = chat.GetLastMessage();

            var text = ClearMessageFromCommand(message);

            if (chat.Dictionary.ContainsKey(text))
            {
                chat.Dictionary.Remove(text);
                return true;
            }

            return false; 
        }

        public string ReturnText()
        { 
            return "Слово успешно удалено!";
        }


    }
}
