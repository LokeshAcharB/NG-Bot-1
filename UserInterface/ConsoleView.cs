using System;
using Unity;
using Unity.Lifetime;
using DataAccess;
using BusinessLogic;
using System.Collections.Generic;

namespace UserInterface
{
    public class ConsoleView : IViewer
    {
        private readonly ProjectValidator projectValidator;
        private readonly EmployeeValidator employeeValidator;
        public ConsoleView()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IEmployeeRepository, EmployeeRepository>(new ContainerControlledLifetimeManager());
            employeeValidator = container.Resolve<EmployeeValidator>();
            container.RegisterType<IProjectRepository, ProjectRepository>(new ContainerControlledLifetimeManager());
            projectValidator = container.Resolve<ProjectValidator>();
        }
        #region Employee Site
        public bool AskNewEmployeeDetails(string ProjectName)
        {
            Employee Employee = new Employee
            {
                ID = Guid.NewGuid()
            };
            Console.Write("\n>>Enter the new employee details\nName : ");
            Employee.FullName = Console.ReadLine().Trim();
            Console.Write("PUID : ");
            Employee.PUID = Console.ReadLine().Trim();
            Console.WriteLine("JobTitle : ");
            Console.WriteLine("1->Senior Technical Lead\n2->Business Analyst\n" +
                                "3->Software Developer\n4->QA Tester");
            Console.Write("Enter the right job number : ");
            Employee.JobTitle = ValidatingKeys.AssigningJob();
            Employee.EmailID = "";

            return employeeValidator.ValidateToAdd(Employee, ProjectName);
        }
        public void AskEmployeePUID()
        {
            Console.Write(">>Enter PUID to fetch his/her details : ");
        AskAgain:
            string PUID = Console.ReadLine().Trim();
            var Employee = (Employee)employeeValidator.ValidateToRead(PUID);
            if (Employee == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("WARNING : ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Please provide valid PUID : ");
                goto AskAgain;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Employee profile");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Name   : {Employee.FullName}");
                Console.WriteLine($"PUID   : {Employee.PUID}");
                Console.WriteLine($"e-Mail : {Employee.EmailID}\nUnder Application : ");
                foreach (Project application in Employee.Projects)
                {
                    Console.WriteLine($"    {application.ProjectName}");
                }
            }
        }
        public bool ModifyEmployeeDetails()
        {
            Employee Employee = new Employee();
            string PUID = string.Empty;
            Console.Write(">>Enter the employee PUID : ");
            PUID = Console.ReadLine().Trim();
            Console.Write("New Name : ");
            Employee.FullName = Console.ReadLine().Trim();
            Console.Write("New PUID : ");
            Employee.PUID = Console.ReadLine().Trim();
            Employee.EmailID = "";
            bool Status = employeeValidator.ValidateToModify(PUID, Employee);
            return Status;
        }
        public bool RemoveEmployee()
        {
            Console.Write("Enter employee PUID to remove : ");
            string RemovePUID = Console.ReadLine().Trim();
            bool Status = employeeValidator.ValidateToDelete(RemovePUID);
            return Status;
        }
        #endregion

        #region Project Site
        public void ViewNGteams()
        {
            var NGteams = projectValidator.ReadNGteams();
            var number = 0;
            foreach (var Team in NGteams)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  {++number}-{Team.TeamName}");
                Console.ForegroundColor = ConsoleColor.White;
                foreach (var Project in Team.Project)
                {
                    Console.WriteLine($"    - {Project.ProjectName}");
                }
            }
        }
        public bool AskNewQuery(string ProjectName)
        {
            Console.Write("Whats new Query?\n-> ");
            string query = Console.ReadLine().Trim();
            Console.Write("Any resolution?\n->");
            string resolution = Console.ReadLine().Trim();
            bool Inserted = projectValidator.ValidateNewQuery(ProjectName, query, resolution);
            return Inserted;
        }
        public void ShowQueryDetails(string ProjectName)
        {
            List<Query> Queries = projectValidator.ReadActiveQueries(ProjectName);
            if (Queries == null || Queries.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Congarts no Active Queries :)");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
            Console.Write(">> Would u prefer to see in Console or Excel sheet : ");
            if (Console.ReadLine().Trim().ToLower().StartsWith("c"))
            {
                int Querynumber = 0;
                foreach (Query query in Queries)
                {
                    if (query.QueryStatus == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write($"Query  ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"{Querynumber++}-{query.QueryName}");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Resolutions :");
                        Console.ForegroundColor = ConsoleColor.White;
                        int ResolutionNumber = 0;
                        foreach (Resolution solution in query.Resolution)
                        {
                            if (solution.ResolutionStatus == 1)
                                Console.WriteLine($"    {ResolutionNumber++}-{solution.Solution}");
                        }
                    }
                }
                return;
            }
            projectValidator.ReadActiveQueriesForXLSheet(ProjectName);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Check the Excel sheet with name '{ProjectName.ToUpper()} Queries.xlsx' in Downloads folder");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public bool ModifyQuery(string ProjectName)
        {
            Console.Write("Enter the Query-nmber : ");
            string Querynumber = Console.ReadLine().Trim();
            Console.Write("Add new Solution : ");
            string solution = Console.ReadLine().Trim();
            bool Modified = projectValidator.ValidateToUpdateQuery(ProjectName, Querynumber, solution);
            return Modified;
        }
        public bool RemoveQuery(string ProjectName)
        {
            Console.Write("Enter the Query-Number : ");
            string Querynumber = Console.ReadLine().Trim();
            Console.Write("Resolution number : ");
            string Resolutionnumber = Console.ReadLine().Trim();
            return projectValidator.ValidateToRemoveQuery(ProjectName, Querynumber, Resolutionnumber);
        }
        public string MultipleProjects(string teamName)
        {
            string Result = null;
            Result = projectValidator.ValidateToCheckMultipleProjects(teamName);
            if (Result.EndsWith("exist !"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Result);
                Console.ForegroundColor = ConsoleColor.White;
                return "0";
            }
            return Result;
        }
        public bool AskNewProjectDetails()
        {
            Console.Write("Under which Team : ");
            string TeamName = Console.ReadLine().Trim();
            Console.Write("Enter the new project name : ");
            string projectName = Console.ReadLine();
            return projectValidator.ValidateToInsertProject(TeamName, projectName);
        }
        public bool RemoveProject()
        {
            Console.Write("Enter the project name : ");
            string ProjectName = Console.ReadLine().Trim();
            bool Status = projectValidator.ValidateToRemoveProject(ProjectName);
            return Status;
        }
        #endregion
    }
}
