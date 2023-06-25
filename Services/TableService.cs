using System.Text.Encodings.Web;
using System.Text.Json;
using StarkovTestApp.DbLayer;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    /// <summary>
    /// Сервис для вывода информации о таблицах
    /// </summary>
    public class TableService : ITableService
    {
        private readonly DbLayerContext _dbLayerContext;

        public TableService(DbLayerContext dbLayerContext)
        {
            _dbLayerContext = dbLayerContext;
        }

        /// <summary>
        /// Метод для формирования Json-файла на основании структуры таблицы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string ExportTable<T>() where T : class
        {
            // устанавливаем нужную таблицу
            var dbSet = _dbLayerContext.Set<T>();

            // поднимаем все сущности
            var entities = dbSet.ToList();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            // сериализуем
            return JsonSerializer.Serialize(entities, options);
        }
    }
}
