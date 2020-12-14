using NLog;
using System;
using System.Linq;
using System.Xml.Linq;

namespace UserInterface
{
    public class ProjectUI : IUserInterface
    {
        private static Logger logger;
        private static IViewer _viewer;
        public ProjectUI(IViewer viewer)
        {
            logger = LogManager.GetCurrentClassLogger();
            _viewer = viewer;
        }
        public void CommandLoop(string ProjectName)
        {
        ProjectMenu:
            PrintProjectMenu();
        AgainEnterKey:
            var key = Console.ReadLine().Trim();
            var result = ValidatingKeys.IsValidKey(key);
            if (result.Item1 && result.Item2 >= 0 && result.Item2 < 7)
            {
                bool Status;
                switch (result.Item2)
                {
                    case 1:
                        Status = _viewer.AskNewQuery(ProjectName);
                        Check(Status);
                        goto ProjectMenu;
                    case 2:
                        _viewer.ShowQueryDetails(ProjectName);
                        goto ProjectMenu;
                    case 3:
                        Status = _viewer.ModifyQuery(ProjectName);
                        Check(Status);
                        goto ProjectMenu;
                    case 4:
                        Status = _viewer.RemoveQuery(ProjectName);
                        Check(Status);
                        goto ProjectMenu;
                    case 5:
                        Status = _viewer.AskNewProjectDetails();
                        Check(Status);
                        goto ProjectMenu;
                    case 6:
                        Status = _viewer.RemoveProject();
                        Check(Status);
                        goto ProjectMenu;
                    case 0:
                        return;
                }
            }
            logger.Info("Provide right value/key to perform operation on application!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Enter the RIGHT key(numberic)!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Try again :)\nKey : ");
            goto AgainEnterKey;
        }
        private void PrintProjectMenu()
        {
            var Option = (from index in XDocument.Load("./Others/Indexes.xml").Descendants("Index")
                          where index.Attribute("level").Value == "ProjectIndex"
                          select new
                          {
                              Ask = index.Element("Ask").Value,
                              NewQuery = index.Element("NewQuery").Value,
                              Read = index.Element("Read").Value,
                              Update = index.Element("Update").Value,
                              Remove = index.Element("Remove").Value,
                              NewProject = index.Element("NewProject").Value,
                              RemoveProject = index.Element("RemoveProject").Value,
                              Back = index.Element("Back").Value,
                          }).FirstOrDefault();
            Console.Write($"\n{Option.Ask}\n{Option.NewQuery}\n{Option.Read}\n{Option.Update}\n" +
                $"{Option.Remove}\n{Option.NewProject}\n{Option.RemoveProject}\n{Option.Back}\nKey : ");
        }
        private static void Check(bool status)
        {
            if (status)
            {
                logger.Info("Succefully Manipulated project!");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(">>Successful :)");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            logger.Error("Something went wroung!");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(">>Please provide VALID data this time :(");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
