using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Bot
{
    public class BotMessageLogic
    {
        private Messenger _messenger;

        private readonly Dictionary<long, Conversation> _chatList;

        private ITelegramBotClient _botClient;

        public BotMessageLogic(ITelegramBotClient botClient)
        {
            _botClient = botClient;
            _messenger = new Messenger(botClient);
            _chatList = new Dictionary<long, Conversation>();
        }

        public async Task Response(MessageEventArgs e)
        {
            var id = e.Message.Chat.Id;

            if (!_chatList.ContainsKey(id))
            {
                var newchat = new Conversation(e.Message.Chat);

                _chatList.Add(id, newchat);
            }

            var chat = _chatList[id];

            chat.AddMessage(e.Message);

            await SendMessage(chat);

        }

        private async Task SendMessage(Conversation chat)
        {
            await _messenger.MakeAnswer(chat);
            
        }


    }
}
