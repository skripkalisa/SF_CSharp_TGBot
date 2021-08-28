using Telegram.Bot;
using Telegram.Bot.Args;

namespace Bot
{
   public class BotWorker
    {
        private ITelegramBotClient _botClient;
        private BotMessageLogic _logic;

        public void Inizalize()
        {
            _botClient = new TelegramBotClient(BotCredentials.BotToken);
            _logic = new BotMessageLogic(_botClient);

        }

        public void Start()
        {
            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();
        }

        public void Stop()
        {
            _botClient.StopReceiving();
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message != null)
            {
                await _logic.Response(e);
            }
        }
    }
}
