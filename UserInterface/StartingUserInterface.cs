using System;
using System.Linq;
using System.Xml.Linq;
using Unity;
using NLog;
using Unity.Lifetime;

namespace PresentationLayer
{
    class StartingUserInterface
    {
        private readonly Logger logger;
        private readonly IUnityContainer container;
        private const string Path = @"C:\Users\DN5W\Documents\Visual Studio 2017\Projects\NG-Bot\UserInterface\Indexes.xml";

        public StartingUserInterface()
        {
            logger = LogManager.GetCurrentClassLogger();
            container = new UnityContainer();
            container.RegisterType<IViewer, ConsoleView>(new ContainerControlledLifetimeManager());
        }
        internal void CommandLoop()
        {
        MainMenu:
            PrintMainMenu();
        AgainEnterKey:
            var key = Console.ReadLine().Trim();
            var result = ValidatingKeys.IsValidKey(key);
            if (result.Item1 && result.Item2 >= 0 && result.Item2 < 3)
            {
                switch (result.Item2)
                {
                    case 1:
                        HaveQuery();
                        goto MainMenu;
                    case 2:
                        var ConsoleView = container.Resolve<ConsoleView>();
                        ConsoleView.ViewNGteams();
                        goto MainMenu;
                    default:
                        return;
                }
            }
            logger.Info("Provide right key to navigate ur'self (1-> Have query, 2-> View NG teams, 0-> exit)!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("WARNING : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Try again with right numeric key \nKey : ");
            goto AgainEnterKey;
        }
        private void PrintMainMenu()
        {
            const string Uri = Path;
            var Option = (from index in XDocument.Load(Uri).Descendants("Index")
                          where index.Attribute("level").Value == "MainIndex"
                          select new
                          { 
                              Query = index.Element("Query").Value,
                              Show = index.Element("Show").Value,
                              Exit = index.Element("Exit").Value
                          }).FirstOrDefault();
            Console.Write($"\n{Option.Query}\n{Option.Show}\n{Option.Exit}\nKey: ");
        }
        private void HaveQuery()
        {
            var ConsoleView = container.Resolve<ConsoleView>();
            Console.Write("Which team u have query?\n   Provide Team Name : ");
            var TeamName = Console.ReadLine().Trim().ToLower();
            switch (Convert.ToInt32(ConsoleView.MultipleProjects(TeamName)))
            {
                case 0:
                    return;
                case 1:
                    SubCommandLoop(TeamName);
                    break;
                default:
                    Console.Write("Which project?\n   Provide Project Name : ");
                    SubCommandLoop(Console.ReadLine().Trim().ToLower());
                    break;
            }
        }
        private void SubCommandLoop(string projectName)
        {
        SubMenu:
            PrintSubMenu();
        AgainEnterKey:
            var key = Console.ReadLine().Trim();
            var result = ValidatingKeys.IsValidKey(key);
            if (result.Item1 && result.Item2 >= 0 && result.Item2 < 3)
            {
                switch (result.Item2)
                {
                    case 1:
                        EmployeeUI employeeUI = container.Resolve<EmployeeUI>();
                        employeeUI.CommandLoop(projectName);
                        goto SubMenu;
                    case 2:
                        ProjectUI projectUI = container.Resolve<ProjectUI>();
                        projectUI.CommandLoop(projectName);
                        goto SubMenu;
                    default:
                        return;
                }
            }
            logger.Info("Provide right key to navigate ur'self (1-> Employee Site, 2-> Project Site, 0-> exit)!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("WARNING : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Try again with right numeric key \nKey : ");
            goto AgainEnterKey;
        }
        private void PrintSubMenu()
        {
            const string Uri = Path;
            var Option = (from index in XDocument.Load(Uri).Descendants("Index")
                          where index.Attribute("level").Value == "SubIndex"
                          select new
                          {
                              Ask = index.Element("Ask").Value,
                              Employee = index.Element("Employee").Value,
                              Project = index.Element("Project").Value,
                              Back = index.Element("Back").Value
                          }).FirstOrDefault();
            Console.Write($"\n{Option.Ask}\n{Option.Employee}\n{Option.Project}\n{Option.Back}\nKey: ");
        }
    }
}

