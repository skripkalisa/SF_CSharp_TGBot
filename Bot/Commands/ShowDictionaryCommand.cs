using System.Collections.Generic;
using Bot.EnglishTrainer.Model;

namespace Bot.Commands
{
    public class ShowDictionaryCommand: AbstractCommand, IChatTextCommand
    {        

        private readonly Dictionary<long, Word> _buffer;
        
        public string ReturnText()
        {
            foreach (var word in _buffer)
            {
                return word.ToString();
            }

            return "Not found";
        }
        
        public ShowDictionaryCommand()
        {
            CommandText = "/dictionary";


            ReturnText();

        }


        
    }
}