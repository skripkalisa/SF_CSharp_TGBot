using Telegram.Bot.Types.ReplyMarkups;

namespace Bot.Commands
{
    interface IKeyBoardCommand
    {
        InlineKeyboardMarkup ReturnKeyBoard();

        void AddCallBack(Conversation chat);

        string InformationalMessage();

    }
}
