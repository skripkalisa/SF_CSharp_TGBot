using System.Threading.Tasks;
using Bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot
{
    public class Messenger
    {
        private readonly ITelegramBotClient _botClient;
        private readonly CommandParser _parser;

        public Messenger(ITelegramBotClient botClient)
        {
            _botClient = botClient;
            _parser = new CommandParser();

            RegisterCommands();
        }

        private void RegisterCommands()
        {
            _parser.AddCommand(new SayHiCommand());
            _parser.AddCommand(new PoemButtonCommand(_botClient));
            _parser.AddCommand(new AddWordCommand(_botClient));
            _parser.AddCommand(new DeleteWordCommand());
            _parser.AddCommand(new TrainingCommand(_botClient));
            _parser.AddCommand(new ShowDictionaryCommand());
            _parser.AddCommand(new StopTrainingCommand());
        }

        public async Task MakeAnswer(Conversation chat)
        {
            var lastMessage = chat.GetLastMessage();

            if (chat.IsTrainingInProcess && !_parser.IsTextCommand(lastMessage))
            {
                _parser.ContinueTraining(lastMessage, chat);

                return;
            }

            if (chat.IsAddingInProcess)
            {
                _parser.NextStage(lastMessage, chat);

                return;
            }

            if (_parser.IsMessageCommand(lastMessage))
            {
                await ExecCommand(chat, lastMessage);
            }
            else
            {
                var text = CreateTextMessage();

                await SendText(chat, text);
            }

        }


        private async Task ExecCommand(Conversation chat, string command)
        {
            if (_parser.IsTextCommand(command))
            {
                var text = _parser.GetMessageText(command, chat);

                await SendText(chat, text);
            }

            if (_parser.IsButtonCommand(command))
            {
                var keys = _parser.GetKeyBoard(command);
                var text = _parser.GetInformationalMessage(command);
                _parser.AddCallback(command, chat);

                await SendTextWithKeyBoard(chat, text, keys);

            }
           
            if(_parser.IsAddingCommand(command))
            {
                chat.IsAddingInProcess = true; 
                _parser.StartAddingWord(command, chat);
            }
        }

        private string CreateTextMessage()
        {
            var text = "Not a command";

            return text;
        }

        private async Task SendText(Conversation chat, string text)
        {
            await _botClient.SendTextMessageAsync(
                  chat.GetId(),
                  text
                );
        }

        private async Task SendTextWithKeyBoard(Conversation chat, string text, InlineKeyboardMarkup keyboard)
        {
            await _botClient.SendTextMessageAsync(
                  chat.GetId(),
                  text,
                  replyMarkup: keyboard
                );
        }
    }
}
