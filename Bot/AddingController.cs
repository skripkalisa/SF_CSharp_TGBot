using System.Collections.Generic;
using Bot.EnglishTrainer.Model;

namespace Bot
{
    public class AddingController
    {
        private readonly Dictionary<long, AddingState> _chatAdding;

        public AddingController()
        {
            _chatAdding = new Dictionary<long, AddingState>();
        }

        public void AddFirstState(Conversation chat)
        {
            _chatAdding.Add(chat.GetId(), AddingState.Russian);
        }

        public void NextStage(string message, Conversation chat)
        {
            var currentState = _chatAdding[chat.GetId()];
            _chatAdding[chat.GetId()] = currentState + 1;

            if (_chatAdding[chat.GetId()] == AddingState.Finish)
            {
                chat.IsAddingInProcess = false;
                _chatAdding.Remove(chat.GetId());
            }
        }

        public AddingState GetStage(Conversation chat)
        {
            return _chatAdding[chat.GetId()];
        }

    }
}
