using System.Collections.Generic;
using Bot.EnglishTrainer.Model;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Commands
{
    public class TrainingCommand : AbstractCommand, IKeyBoardCommand
    {

        private readonly ITelegramBotClient _botClient;

        private readonly Dictionary<long, TrainingType> _training;

        private readonly Dictionary<long, Conversation> _trainingChats;

        private readonly Dictionary<long, string> _activeWord;

        public TrainingCommand(ITelegramBotClient botClient)
        {
            CommandText = "/training";

            _botClient = botClient;

            _training = new Dictionary<long, TrainingType>();
            _trainingChats = new Dictionary<long, Conversation>();
            _activeWord = new Dictionary<long, string>();
        }

        public InlineKeyboardMarkup ReturnKeyBoard()
        {
            var buttonList = new List<InlineKeyboardButton>
            {
                new()
                {
                    Text = "С русского на английский",
                    CallbackData = "rustoeng"
                },

                new()
                {
                    Text = "С английского на русский",
                    CallbackData = "engtorus"
                }
            };

            var keyboard = new InlineKeyboardMarkup(buttonList);

            return keyboard;
        }

        public string InformationalMessage()
        {
            return "Выберите тип тренировки. Для окончания тренировки введите команду /stop";
        }

        public void AddCallBack(Conversation chat)
        {
            _trainingChats.Add(chat.GetId(), chat);

            _botClient.OnCallbackQuery -= Bot_Callback;
            _botClient.OnCallbackQuery += Bot_Callback;
        }

        private async void Bot_Callback(object sender, CallbackQueryEventArgs e)
        {
            var text = "";

            var id = e.CallbackQuery.Message.Chat.Id;

            var chat = _trainingChats[id];

            switch (e.CallbackQuery.Data)
            {
                case "rustoeng":
                    _training.Add(id, TrainingType.RusToEng);

                    text =  chat.GetTrainingWord(TrainingType.RusToEng);

                    break;
                case "engtorus":
                    _training.Add(id, TrainingType.EngToRus);

                    text = chat.GetTrainingWord(TrainingType.EngToRus);
                    break;
            }

            chat.IsTrainingInProcess = true;
            _activeWord.Add(id, text);

            if (_trainingChats.ContainsKey(id))
            {
                _trainingChats.Remove(id);
            }

            await _botClient.SendTextMessageAsync(id, text);
            await _botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
        }

        public async void NextStepAsync(Conversation chat, string message)
        {
            var type = _training[chat.GetId()];
            var word = _activeWord[chat.GetId()];

            var check = chat.CheckWord(type, word, message);

            string text;

            if (check)
            {
                text = "Правильно!";
            }
            else
            {
                text = "Неправильно!";
            }

            text = text +  " Следующее слово: ";

            var newWord = chat.GetTrainingWord(type);

            text = text + newWord;

            _activeWord[chat.GetId()] = newWord;


            await _botClient.SendTextMessageAsync(chat.GetId(), text);
        }
    }
}
