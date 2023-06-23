namespace StarkovTestApp.Services.Interfaces
{
    public interface IUploadService
    {
        string UploadEmployees(string file);
        string UploadDepartments(string file);
        string UploadJobTitles(string file);

    }
}
