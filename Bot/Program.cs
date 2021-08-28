using System;

namespace Bot
{
    static class Program
    {

        static void Main()
        {
            var bot = new BotWorker();

            bot.Inizalize();
            bot.Start();

            Console.WriteLine("Напишите stop для прекращения работы");

            string command;
            do
            {
                command = Console.ReadLine();

            } while (command != "stop") ;

            bot.Stop();

        }

       
    }
}
