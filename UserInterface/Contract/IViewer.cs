﻿namespace PresentationLayer
{
    public interface IViewer
    {
        bool AskNewEmployeeDetails();
        bool RemoveEmployee();
        bool ModifyEmployeeDetails();
        void AskEmployeePUID();
        bool AskNewQuery(string ProjectName);
        void ShowQueryDetails(string ProjectName);
        bool ModifyQuery(string ProjectName);
        bool RemoveQuery(string ProjectName);
        bool AskNewProjectDetails();
        bool RemoveProject();
        string MultipleProjects(string teamName);
        void ViewNGteams();
    }
}
