using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StarkovTestApp.DbLayer;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    /// <summary>
    /// Сервис для проставления связей между таблицами
    /// </summary>
    public class LinkService : ILinkService
    {
        private readonly DbLayerContext _dbLayerContext;

        public LinkService(DbLayerContext dbLayerContext)
        {
            _dbLayerContext = dbLayerContext;

        }

        /// <summary>
        /// Метод для обновления связей
        /// </summary>
        /// <param name="type"></param>
        public void UpdateTables(Consts.ContentTypes type)
        {
            string[] procedures;
            switch (type)
            {
                // если обновляем таблицу Department, нужно обновить внутренние связи,
                // ссылки на менеджеров и проставить отделы работникам
                case Consts.ContentTypes.Department:
                    procedures = new string[]
                    {
                        Consts.StoredProcedures.UPDATE_MANAGERS_IDS,
                        Consts.StoredProcedures.UPDATE_DEP_IDS,
                        Consts.StoredProcedures.UPDATE_INNER_DEP_IDS
                    };
                    break;
                // если обновляем таблицу Employees, нужно обновить ссылки на отделы,
                // менеджеров отделов, а также ссылки на должности
                case Consts.ContentTypes.Employee:
                    procedures = new string[]
                    {
                        Consts.StoredProcedures.UPDATE_JOB_TITLES,
                        Consts.StoredProcedures.UPDATE_DEP_IDS,
                        Consts.StoredProcedures.UPDATE_MANAGERS_IDS
                    };
                    break;
                // если обновляем таблицу JobTitles, нужно обновить только ссылки на должности таблицы Employees
                default:
                    procedures = new string[]
                    {
                        Consts.StoredProcedures.UPDATE_JOB_TITLES
                    };
                    break;
            }

            ExecuteQuery(procedures);
        }

        /// <summary>
        /// Метод для вызова хранимых процедур
        /// </summary>
        /// <param name="queries"></param>
        private void ExecuteQuery(params string[] queries)
        {
            // открываем соединение
            using (var conn = new NpgsqlConnection(_dbLayerContext.Database.GetConnectionString()))
            {
                conn.OpenAsync();

                // последовательно обрабатываем вызов ХП
                foreach (var query in queries)
                {
                    var command = new NpgsqlCommand(query, conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
