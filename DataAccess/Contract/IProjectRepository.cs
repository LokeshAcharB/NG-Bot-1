using System;
using System.Collections.Generic;

namespace DataAccess
{
    public interface IProjectRepository
    {
        bool AddNewProject(string TeamName, string NewProjectName);
        bool EraseProject(string ProjectName);
        bool AddQuery(string ProjectName, string query, string resolution);
        List<Query> GetQueries(string ProjectName);
        bool UpdateQuery(string ProjectName, int QueryNumber, string resolution);
        bool RemoveQueries(string ProjectName, int QueryNumber, int ResolutionNumber);
        string CheckForMultipleProjects(string teamName);
        List<Team> GetAllNGteams();
        Tuple<List<Query>, List<Resolution>> GetQueriesForXLsheet(string ProjectName);
    }
}
