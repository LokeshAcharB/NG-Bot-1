using CommonLibrary;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class ProjectValidator : IProjectValidator
    {
        private readonly IProjectRepository _ProjectRepository;
        public ProjectValidator(IProjectRepository projectRepository)
        {
            _ProjectRepository = projectRepository;
        }
        public bool ValidateToInsertProject(string TeamName, string ProjectName)
        {
            bool Valid = !string.IsNullOrEmpty(ProjectName) && !string.IsNullOrEmpty(TeamName) && 
                         ProjectName.IsAlphaNumeric() && TeamName.IsAlphaNumeric();
            return Valid ? _ProjectRepository.AddNewProject(TeamName, ProjectName) : false;
        }
        public bool ValidateToRemoveProject(string ProjectName)
        {
            bool Valid = !string.IsNullOrEmpty(ProjectName) && ProjectName.IsAlphaNumeric();
            return Valid ? _ProjectRepository.EraseProject(ProjectName) : false;
        }
        public bool ValidateNewQuery(string ProjectName, string query, string resolution)
        {
            bool Valid = !string.IsNullOrEmpty(ProjectName) && !string.IsNullOrEmpty(query) 
                && !string.IsNullOrEmpty(resolution) && ProjectName.IsAlphaNumeric();
            return Valid ? _ProjectRepository.AddQuery(ProjectName, query, resolution) : false;
        }
        public List<Query> ReadActiveQueries(string ProjectName)
        {
            bool Valid = !string.IsNullOrEmpty(ProjectName) && ProjectName.IsAlphaNumeric();
            return Valid ? _ProjectRepository.GetQueries(ProjectName) ?? null : null;
        }
        public void ReadActiveQueriesForXLSheet(string ProjectName)
        {
            var (Queries, Resolutions) = _ProjectRepository.GetQueriesForXLsheet(ProjectName);
            try
            {
                var Result = (from Resolution in Resolutions
                              join Query in Queries
                              on Resolution.QueryID equals Query.QueryID
                              select new
                              {
                                  QueryStatement = Query.QueryName,
                                  Resolution = Resolution.Solution
                              }).AsEnumerable();
                var mapper = new Ganss.Excel.ExcelMapper();
                var Path = $@"C:\Users\DN5W\Documents\Visual Studio 2017\Projects\NG-Bot\Queries\{ProjectName.ToUpper()} Queries.xlsx";
                mapper.Save(Path, Result, $"{ProjectName} Queries", true);
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("\n\tPlease close the file and try again!");
            }
        }
        public bool ValidateToUpdateQuery(string ProjectName, string Querynumber, string resolution)
        {
            int QueryNumber = 0;
            bool Valid = ProjectName.IsAlphaNumeric() && !string.IsNullOrEmpty(resolution) && int.TryParse(Querynumber, out QueryNumber);
            return  Valid ? _ProjectRepository.UpdateQuery(ProjectName, QueryNumber, resolution) : false;
        }
        public bool ValidateToRemoveQuery(string ProjectName, string Querynumber, string Resolutionnumber)
        {
            int QueryNumber = 0;
            int ResolutionNumber = 0;
            bool Valid = ProjectName.IsAlphaNumeric() && int.TryParse(Resolutionnumber, out ResolutionNumber) 
                && int.TryParse(Querynumber, out QueryNumber);
            return Valid ? _ProjectRepository.RemoveQueries(ProjectName, QueryNumber, ResolutionNumber) : false;
        }
        public string ValidateToCheckMultipleProjects(string teamName)
        {
            return teamName.IsAlphaNumeric() ? _ProjectRepository.CheckForMultipleProjects(teamName): "Team name should be AlphaNumeric";
        }
        public List<Team> ReadNGteams()
        {
            return _ProjectRepository.GetAllNGteams();
        }
    }
}
