namespace StarkovTestApp.Services.Interfaces
{
    public interface IUploadService
    {
        void UploadEmployees(string file);
        void UploadDepartments(string file);
        void UploadJobTitles(string file);

    }
}
