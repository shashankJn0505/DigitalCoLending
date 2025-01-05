using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Logs
{
    public class SqlDatabaseUtility
    {

        public async Task<DataTable> ExecuteCommandAsync(string connectionName, string storedProcName, List<SqlParameter> procParameters)
        {
            DataTable dt = new DataTable();

            SqlConnection cn = new SqlConnection(connectionName);
            await cn.OpenAsync();
            // create a SQL command to execute the stored procedure
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcName;

            // assign parameters passed in to the command
            foreach (var procParameter in procParameters)
            {
                cmd.Parameters.Add(procParameter);
            }
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            await cn.CloseAsync();

            return dt;
        }
    }

}
