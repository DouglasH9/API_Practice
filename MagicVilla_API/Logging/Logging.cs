using System;
namespace MagicVilla_API.Logging
{
    public class Logging : ILogging
    {
        public Logging()
        {
        }

        public void Log(string message, LogType type)
        {
            if (type is LogType.ERROR)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(type + "- " + message);
            }
            else if (type is LogType.WARNING)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(type + "- " + message);
            }
            else if (type is LogType.INFORMATION)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine(type + "- " + message);
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine(type + "- " + message);
            }
            
        }
    }

    public enum LogType
    {
        DEBUG,
        TRACE,
        INFORMATION,
        WARNING,
        ERROR
    }
}

