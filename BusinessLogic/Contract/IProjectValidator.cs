using DataAccess;
using System.Collections.Generic;

namespace BusinessLogic
{
    interface IProjectValidator
    {
        bool ValidateToInsertProject(string TeamName, string ProjectName);
        bool ValidateToRemoveProject(string ProjectName);
        bool ValidateToRemoveQuery(string ProjectName, string Querynumber, string Resolutionnumber);
        List<Query> ReadActiveQueries(string ProjectName);
        void ReadActiveQueriesForXLSheet(string ProjectName);
        bool ValidateNewQuery(string ProjectName, string query, string resolution);
        bool ValidateToUpdateQuery(string ProjectName, string Querynumber, string resolution);
        List<Team> ReadNGteams();
    }
}
