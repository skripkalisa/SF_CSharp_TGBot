using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.EnglishTrainer.Model;
using Telegram.Bot;

namespace Bot.Commands
{
    public class AddWordCommand : AbstractCommand
    {

        private readonly ITelegramBotClient _botClient;

        private readonly Dictionary<long, Word> _buffer;

        public AddWordCommand(ITelegramBotClient botClient)
        {
            CommandText = "/addword";

            _botClient = botClient;

            _buffer = new Dictionary<long, Word>();
        }

        public async void StartProcessAsync(Conversation chat)
        {
            _buffer.Add(chat.GetId(), new Word());

            var text = "Введите русское значение слова";

            await SendCommandText(text, chat.GetId());
        }

        public async void DoForStageAsync(AddingState addingState, Conversation chat, string message)
        {
            var word = _buffer[chat.GetId()];
            var text = "";

            switch (addingState)
            {
                case AddingState.Russian:
                    word.Russian = message;

                    text = "Введите английское значение слова";
                    break;

                case AddingState.English:
                    word.English = message;

                    text = "Введите тематику";
                    break;

                case AddingState.Theme:
                    word.Topic = message;

                    text = "Успешно! Слово " + word.English + " добавлено в словарь. ";

                    chat.Dictionary.Add(word.Russian, word);

                    _buffer.Remove(chat.GetId());
                    break;
            }


            await SendCommandText(text, chat.GetId());
        }

        private async Task SendCommandText(string text, long chat)
        {
            await _botClient.SendTextMessageAsync(chat, text);
        }

    }
}
