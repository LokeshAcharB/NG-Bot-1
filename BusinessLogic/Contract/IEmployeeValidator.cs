namespace BusinessLogic
{
    public interface IEmployeeValidator
    {
        bool ValidateToUpload(object obj);
        object ValidateToRead(string PUID);
        bool ValidateToModify(string OldPUID, object NewEmploye);
        bool ValidateToDelete(string PUID);
    }
}
