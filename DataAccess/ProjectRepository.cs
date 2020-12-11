using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly Logger logger;
        private readonly EmployeeDbEntities EmployeeDbcontext;
        public ProjectRepository()
        {
            logger = LogManager.GetCurrentClassLogger();
            EmployeeDbcontext = new EmployeeDbEntities();
        }
        public bool AddNewProject(string TeamName, string NewProjectName)
        {
            try //To check if new entry exists already
            {
                Project Proj = (from team in EmployeeDbcontext.Teams.Include("Project")
                                from project in team.Project
                                where team.TeamName == TeamName && project.ProjectName == NewProjectName
                                select project).AsEnumerable().FirstOrDefault();
                Proj.ProjectStatus = 1;
                EmployeeDbcontext.SaveChanges();
                return true;
            }
            catch (NullReferenceException) // To add the new entry
            {
                Team Team = (from team in EmployeeDbcontext.Teams
                             where team.TeamName == TeamName
                             select team).AsEnumerable().FirstOrDefault();
                Team.TeamStatus = 1;
                var Project = new Project() { TeamID = Team.TeamID, ProjectID = Guid.NewGuid(), ProjectName = NewProjectName, ProjectStatus = 1 };
                EmployeeDbcontext.Projects.Add(Project);
                EmployeeDbcontext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error($"Something bad happened in query site {ex}!");
                return false;
            }
        }
        public bool EraseProject(string ProjectName)
        {
            try
            {
                var project = (from proj in EmployeeDbcontext.Projects
                                   where proj.ProjectName == ProjectName
                                   select proj).AsEnumerable().FirstOrDefault();
                project.ProjectStatus = 0;
                foreach (Query query in project.Query)
                {
                    query.QueryStatus = 0;
                }
                EmployeeDbcontext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error($"Something bad happened in Project site {ex}!");
                return false;
            }
        }
        public bool AddQuery(string ProjectName, string query, string resolution)
        {
            var Query = new Query()
            {
                QueryID = Guid.NewGuid(),
                QueryName = query,
                QueryStatus = 1
            };
            var Resolution = new Resolution()
            {
                ID = Guid.NewGuid(),
                QueryID = Query.QueryID,
                Solution = resolution,
                ResolutionStatus = 1
            };
            try
            {
                var project = (from proj in EmployeeDbcontext.Projects
                               where proj.ProjectName == ProjectName
                               select proj).AsEnumerable().FirstOrDefault();
                project.ProjectStatus = 1;
                Query.ProjectID = project.ProjectID;
                EmployeeDbcontext.Queries.Add(Query);
                EmployeeDbcontext.Resolutions.Add(Resolution);
                EmployeeDbcontext.SaveChanges();
                return true;
            }
            catch (NullReferenceException ex)
            {
                logger.Error($"Something bad happened in Query site (Entered project dose not exist) {ex}");
                return false;
            }
        }
        public List<Query> GetQueries(string ProjectName)
        {
            try
            {
                var Project = (from proj in EmployeeDbcontext.Projects
                               where proj.ProjectName == ProjectName
                               select proj).AsEnumerable().FirstOrDefault();
                var Queries = (from query in EmployeeDbcontext.Queries.Include("Resolution")
                               where query.ProjectID == Project.ProjectID
                               select query).AsEnumerable();
                return Queries.ToList() ?? new List<Query>();
            }
            catch (Exception ex)
            {
                logger.Error($"Something bad happened in Query site {ex}!");
                return new List<Query>();
            }
        }
        public Tuple<List<Query>, List<Resolution>> GetQueriesForXLsheet(string ProjectName)
        {
            try
            {
                var Query = (from query in EmployeeDbcontext.Queries
                             where query.QueryStatus == 1 && query.Project.ProjectName == ProjectName
                             select query).AsEnumerable().ToList();
                var Resolution = (from resolution in EmployeeDbcontext.Resolutions
                                  where resolution.ResolutionStatus == 1
                                  select resolution).AsEnumerable().ToList();
                return Tuple.Create(Query, Resolution);
            }
            catch (Exception ex)
            {
                logger.Error($"Something bad happened in Query site {ex}!");
                return null;
            }
        }
        public bool UpdateQuery(string ProjectName, int QueryNumber, string resolution)
        {
            try
            {
                var Query = (from project in EmployeeDbcontext.Projects
                               where project.ProjectName == ProjectName
                               from query in project.Query
                               where query.QueryStatus == 1 && query.ProjectID == project.ProjectID
                               select query).AsEnumerable().ElementAtOrDefault(QueryNumber);
                EmployeeDbcontext.Resolutions.Add(new Resolution()
                               { ID = Guid.NewGuid(), QueryID = Query.QueryID, Solution = resolution, ResolutionStatus = 1});
                EmployeeDbcontext.SaveChanges();
                return true;
            }
            catch(IndexOutOfRangeException ex)
            {
                logger.Error($"Something bad happened in Query site {ex}!");
                return false;
            }
            catch (Exception ex)
            {
                logger.Error($"Something bad happened in Query site {ex}!");
                return false;
            }
        }
        public bool RemoveQueries(string ProjectName, int QueryNumber, int ResolutionNumber)
        {
            try
            {
                var Query = (from project in EmployeeDbcontext.Projects.Include("Query")
                             where project.ProjectName == ProjectName
                             from query in project.Query
                             where query.QueryStatus == 1 && query.ProjectID == project.ProjectID
                             select query).AsEnumerable().ElementAtOrDefault(QueryNumber);

                var resolution = (from solution in Query.Resolution
                                  where solution.ResolutionStatus == 1
                                  select solution).AsEnumerable().ElementAtOrDefault(ResolutionNumber);
                resolution.ResolutionStatus = 0;
                CloseQuery(Query); // closes query if no active solutions exists!
                EmployeeDbcontext.SaveChanges();
                return true;
            }
            catch (IndexOutOfRangeException ex)
            {
                logger.Error($"Something bad happened in Query site {ex}!");
                return false;
            }
            catch (Exception ex)
            {
                logger.Error($"Something bad happened in Query site {ex}!");
                return false;
            }
        }
        private void CloseQuery(Query query)
        {
            bool check = true;
            foreach (var solution in query.Resolution)
            {
                check = check && (solution.ResolutionStatus == 0);
            }
            if (check)
                query.QueryStatus = 0;
        }
        public string CheckForMultipleProjects(string teamName)
        {
            try
            {
                var Count = (from team in EmployeeDbcontext.Teams.Include("Project")
                             where team.TeamName == teamName
                             select team).AsEnumerable().FirstOrDefault().Project.Count;
                return Count.ToString();
            }
            catch (Exception)
            {
                return $"{teamName} dosent exist !";
            }
        }
        public List<Team> GetAllNGteams()
        {
            var Teams = (from team in EmployeeDbcontext.Teams.Include("Project")
                         where team.TeamStatus == 1
                         orderby team.TeamName
                         select team).AsEnumerable();
            return Teams.ToList();
        }
    }
}
