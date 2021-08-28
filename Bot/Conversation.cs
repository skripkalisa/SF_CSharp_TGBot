using System;
using System.Collections.Generic;
using System.Linq;
using Bot.EnglishTrainer.Model;
using Telegram.Bot.Types;

namespace Bot
{
    public class Conversation
    {
        private readonly Chat _telegramChat;

        private readonly List<Message> _telegramMessages;

        public readonly Dictionary<string, Word> Dictionary;

        public bool IsAddingInProcess;

        public bool IsTrainingInProcess;

        public Conversation(Chat chat)
        {
            _telegramChat = chat;
            _telegramMessages = new List<Message>();
            Dictionary = new Dictionary<string, Word>();
        }

        public void AddMessage(Message message)
        {
            _telegramMessages.Add(message);
        }

        public void AddWord(string key, Word word)
        {
            Dictionary.Add(key, word);
        }

        public void ClearHistory()
        {
            _telegramMessages.Clear();
        }

        public List<string> GetTextMessages()
        {
            var textMessages = new List<string>();

            foreach(var message in _telegramMessages)
            {
                if (message.Text != null)
                {
                    textMessages.Add(message.Text);
                }
            }

            return textMessages;
        }

        public long GetId() => _telegramChat.Id;

        public string GetLastMessage() => _telegramMessages[^1].Text;

        public string GetTrainingWord(TrainingType type)
        {
            var rand = new Random();
            var item = rand.Next(0, Dictionary.Count);

            var randomWord = Dictionary.Values.AsEnumerable().ElementAt(item);

            var text = string.Empty;

            switch (type)
            {
                case TrainingType.EngToRus:
                    text =  randomWord.English;
                    break;

                case TrainingType.RusToEng:
                    text = randomWord.Russian;
                    break;
            }

            return text;
        }

        public bool CheckWord(TrainingType type, string word, string answer)
        {
            Word control;

            var result = false;

            switch (type)
            {

                case TrainingType.EngToRus:

                    control = Dictionary.Values.FirstOrDefault(x => x.English == word);

                    result = control?.Russian == answer;

                    break;

                case TrainingType.RusToEng:
                    control = Dictionary[word];

                    result = control.English == answer;

                    break;
            }

            return result; 
        }

    }
}
