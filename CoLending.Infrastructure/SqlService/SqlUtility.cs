using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Infrastructure.SqlService
{
    public class SqlUtility : ISqlUtility
    {
        public async Task<DataTable> ExecuteCommandAsync(string connectionName, string storedProcName, List<SqlParameter> procParameters = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection cn = new SqlConnection(connectionName))
            {
                // create a SQL command to execute the stored procedure
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storedProcName;

                if (procParameters != null)
                {
                    // assign parameters passed in to the command
                    foreach (var procParameter in procParameters)
                    {
                        cmd.Parameters.Add(procParameter);
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
                await cn.CloseAsync();
            }

            return dt;
        }

        public async Task<DataSet> ExecuteMultipleCommandAsync(string connectionName, string storedProcName, List<SqlParameter> procParameters = null)
        {
            DataSet ds = new DataSet();

            using (SqlConnection cn = new SqlConnection(connectionName))
            {
                // create a SQL command to execute the stored procedure
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storedProcName;

                if (procParameters != null)
                {
                    // assign parameters passed in to the command
                    foreach (var procParameter in procParameters)
                    {
                        cmd.Parameters.Add(procParameter);
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                await cn.CloseAsync();
            }

            return ds;
        }
    }
}
