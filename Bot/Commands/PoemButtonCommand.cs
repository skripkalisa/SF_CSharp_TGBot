using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Commands
{
    public class PoemButtonCommand : AbstractCommand, IKeyBoardCommand
    {
        readonly ITelegramBotClient _botClient;

        public PoemButtonCommand(ITelegramBotClient botClient)
        {
            _botClient = botClient;

            CommandText = "/poembuttons";
        }

        public void AddCallBack(Conversation chat)
        {
            _botClient.OnCallbackQuery -= Bot_Callback;
            _botClient.OnCallbackQuery += Bot_Callback;
        }

        private async void Bot_Callback(object sender, CallbackQueryEventArgs e)
        {
            var text = "";

            switch (e.CallbackQuery.Data)
            {
                case "pushkin":
                    text = @"Я помню чудное мгновенье:
                    Передо мной явилась ты,
                    Как мимолетное виденье,
                    Как гений чистой красоты."; 
                    break;
                case "esenin":
                    text = @"Не каждый умеет петь,
                    Не каждому дано яблоком
                    Падать к чужим ногам.";
                    break;
            }

            await _botClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, text);
            await _botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
        }

        public InlineKeyboardMarkup ReturnKeyBoard()
        {
            var buttonList = new List<InlineKeyboardButton>
            {
                new()
                {
                    Text = "Пушкин",
                    CallbackData = "pushkin"
                },

                new()
                {
                    Text = "Есенин",
                    CallbackData = "esenin"
                }
            };

            var keyboard = new InlineKeyboardMarkup(buttonList);

            return keyboard;
        }

        public string InformationalMessage()
        {
            return "Выберите поэта";
        }
    }
}
