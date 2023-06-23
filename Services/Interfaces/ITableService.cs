namespace StarkovTestApp.Services.Interfaces
{
    public interface ITableService
    {
        string ExportTable<T>() where T : class;
    }
}
