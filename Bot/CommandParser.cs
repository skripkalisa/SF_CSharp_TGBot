using System.Collections.Generic;
using Bot.Commands;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot
{
    public class CommandParser
    {
        private readonly List<IChatCommand> _command;

        private readonly AddingController _addingController;

        public CommandParser()
        {
            _command = new List<IChatCommand>();
            _addingController = new AddingController();
        }

        public void AddCommand(IChatCommand chatCommand)
        {
            _command.Add(chatCommand);
        }

        public bool IsMessageCommand(string message)
        {
           return _command.Exists(x => x.CheckMessage(message));
        }

        public bool IsTextCommand(string message)
        {
            var command = _command.Find(x => x.CheckMessage(message));

            return command is IChatTextCommand;
        }

        public bool IsButtonCommand(string message)
        {
            var command = _command.Find(x => x.CheckMessage(message));

            return command is IKeyBoardCommand;
        }

        public string GetMessageText(string message, Conversation chat)
        {
            var command = _command.Find(x => x.CheckMessage(message)) as IChatTextCommand;

            if (command is IChatTextCommandWithAction action)
            {
                if (!action.DoAction(chat))
                {
                    return "Ошибка выполнения команды!";
                };
            }

            return command?.ReturnText();
        }

        public string GetInformationalMessage(string message)
        {
            var command = _command.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;

            return command?.InformationalMessage();
        }

        public InlineKeyboardMarkup GetKeyBoard(string message)
        {
            var command = _command.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;

            return command?.ReturnKeyBoard();
        }

        public void AddCallback(string message, Conversation chat)
        {
            var command = _command.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;
            command?.AddCallBack(chat);
        }

        public bool IsAddingCommand(string message)
        {
            var command = _command.Find(x => x.CheckMessage(message));

            return command is AddWordCommand;
        }

        public void StartAddingWord(string message, Conversation chat)
        {
            var command = _command.Find(x => x.CheckMessage(message)) as AddWordCommand;

            _addingController.AddFirstState(chat);
            command?.StartProcessAsync(chat);

        }

        public void NextStage(string message, Conversation chat)
        {
            var command = _command.Find(x => x is AddWordCommand) as AddWordCommand;

            command?.DoForStageAsync(_addingController.GetStage(chat), chat, message);

            _addingController.NextStage(message, chat);

        }


        public void ContinueTraining(string message, Conversation chat)
        {
            var command = _command.Find(x => x is TrainingCommand) as TrainingCommand;

            command?.NextStepAsync(chat, message);

        }

    }
}
