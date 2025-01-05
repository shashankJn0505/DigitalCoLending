using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Infrastructure.SqlService
{
    public interface ISqlUtility
    {
        Task<DataTable> ExecuteCommandAsync(string connectionName, string storedProcName, List<SqlParameter> procParameters = null);

        Task<DataSet> ExecuteMultipleCommandAsync(string connectionName, string storedProcName, List<SqlParameter> procParameters = null);
    }

}
