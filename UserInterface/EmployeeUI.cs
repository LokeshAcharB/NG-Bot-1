using NLog;
using System;
using System.Linq;
using System.Xml.Linq;

namespace UserInterface
{
    public class EmployeeUI : IUserInterface
    {
        private static Logger logger;
        private static IViewer _viewer;
        public EmployeeUI(IViewer viewer)
        {
            logger = LogManager.GetCurrentClassLogger();
            _viewer = viewer;
        }
        public void CommandLoop(string ProjectName)
        {
        SubMenu:
            SubMenu();
        AgainEnterKey:

            var key = Console.ReadLine().Trim();
            var result = ValidatingKeys.IsValidKey(key);
            if (result.Item1 && result.Item2 >= 0 && result.Item2 < 5)
            {
                bool Status;
                switch (result.Item2)
                {
                    case 1:
                        Status = _viewer.AskNewEmployeeDetails(ProjectName);
                        Check(Status);
                        goto SubMenu;
                    case 2:
                        Status = _viewer.RemoveEmployee();
                        Check(Status);
                        goto SubMenu;
                    case 3:
                        Status = _viewer.ModifyEmployeeDetails();
                        Check(Status);
                        goto SubMenu;
                    case 4:
                        _viewer.AskEmployeePUID();
                        goto SubMenu;
                    case 0:
                        return;
                }
            }
            logger.Info("Provide right value/key to perform operation on employee!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Enter the RIGHT key(numberic)!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Try again :)\nKey : ");
            goto AgainEnterKey;
        }
        private void SubMenu()
        {
            var Option = (from index in XDocument.Load("./Others/Indexes.xml").Descendants("Index")
                          where index.Attribute("level").Value == "EmployeeIndex"
                          select new
                          {
                              Ask = index.Element("Ask").Value,
                              Create = index.Element("Create").Value,
                              Read = index.Element("Read").Value,
                              Update = index.Element("Update").Value,
                              Delete = index.Element("Delete").Value,
                              Back = index.Element("Back").Value,

                          }).FirstOrDefault();
            Console.WriteLine($"\n{Option.Ask}\n{Option.Create}\n{Option.Delete}" +
                              $"\n{Option.Update}\n{Option.Read}\n{Option.Back}\nKey : ");
        }
        private static void Check(bool status)
        {
            if (status)
            {
                logger.Info("Succefully Manipulated employee!");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(">>Successful :)");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(">>Please provide VALID data this time :(");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
