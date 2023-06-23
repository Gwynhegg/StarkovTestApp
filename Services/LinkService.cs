using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StarkovTestApp.DbLayer;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    public class LinkService : ILinkService
    {
        private readonly DbLayerContext _dbLayerContext;

        public LinkService(DbLayerContext dbLayerContext)
        {
            _dbLayerContext = dbLayerContext;

        }

        public void UpdateDepartmentTable()
        {
            ExecuteQuery(Consts.StoredProcedures.UPDATE_MANAGERS_IDS,
                Consts.StoredProcedures.UPDATE_DEP_IDS,
                Consts.StoredProcedures.UPDATE_INNER_DEP_IDS);
        }

        public void UpdateEmployeesTable()
        {
            ExecuteQuery(Consts.StoredProcedures.UPDATE_JOB_TITLES,
                Consts.StoredProcedures.UPDATE_DEP_IDS,
                Consts.StoredProcedures.UPDATE_MANAGERS_IDS);
        }

        public void UpdateJobTitlesTable()
        {
            ExecuteQuery(Consts.StoredProcedures.UPDATE_JOB_TITLES);
        }

        private void ExecuteQuery(params string[] queries)
        {
            using (var conn = new NpgsqlConnection(_dbLayerContext.Database.GetConnectionString()))
            {
                conn.OpenAsync();

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
