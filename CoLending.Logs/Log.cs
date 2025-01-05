using CoLending.Core.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Logs
{
    public class Log : ILog
    {
        private readonly ConnectionStringsOptions _connectionStringsOptions;
        public Log(IOptions<ConnectionStringsOptions> connectionStringsOptions)
        {
            _connectionStringsOptions = connectionStringsOptions.Value;
        }
        public async Task<Guid> AddLogs(LogModel logModel)
        {
            SqlDatabaseUtility sqlDatabaseUtility = new SqlDatabaseUtility();

            List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("BpNo", logModel.BpNo),
            new SqlParameter("UserType", logModel.UserType),
            new SqlParameter("Url", logModel.Url),
            new SqlParameter("RequestHttpVerb", logModel.RequestHttpVerb),
            new SqlParameter("RequestBody", logModel.RequestBody),
            new SqlParameter("CreatedBy", logModel.CreatedBy),
            new SqlParameter("CreatedDate", logModel.CreatedDate)
    };

            DataTable dt = await sqlDatabaseUtility.ExecuteCommandAsync(_connectionStringsOptions.DefaultConnection, "usp_add_logs", parameters);
            return (Guid)dt.Rows[0]["Id"];
        }

        public async Task UpdateLogs(LogModel logModel)
        {
            SqlDatabaseUtility sqlDatabaseUtility = new SqlDatabaseUtility();

            List<SqlParameter> parameters = new List<SqlParameter>()
        {
             new SqlParameter("Id", logModel.Id),
            new SqlParameter("ResponseBody", logModel.ResponseBody!),
            new SqlParameter("ResponseStatusCode", logModel.ResponseStatusCode!),
            new SqlParameter("IsSuccess", logModel.IsSuccess),
            new SqlParameter("UpdatedDate", logModel.UpdatedDate)
        };
            await sqlDatabaseUtility.ExecuteCommandAsync(_connectionStringsOptions.DefaultConnection, "usp_update_logs", parameters);
        }
    }

}
