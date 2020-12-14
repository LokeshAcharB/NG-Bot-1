using System;
using System.Linq;

namespace UserInterface
{
    static class Start
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("                   Welcome to NG-Bot");
            Console.WriteLine("---------------------------------------------------------");

            Intro();
            var startingUserInterface = new StartingUserInterface();
            startingUserInterface.CommandLoop();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("                 ThankYou! Visit again");
            Console.ReadKey();
        }
        private static void Intro()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Here, you can have deep insight on queries currently running \n" +
                                "in any team/project in Eurofins! And also get in touch with \n" +
                                "Employee site and Project site of individual eurofins project");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
