using System.Text.Encodings.Web;
using System.Text.Json;
using StarkovTestApp.DbLayer;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    public class TableService : ITableService
    {
        private readonly DbLayerContext _dbLayerContext;

        public TableService(DbLayerContext dbLayerContext)
        {
            _dbLayerContext = dbLayerContext;
        }

        public string ExportTable<T>() where T : class
        {
            var dbSet = _dbLayerContext.Set<T>();

            var entities = dbSet.ToList();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            return JsonSerializer.Serialize(entities, options);
        }
    }
}
