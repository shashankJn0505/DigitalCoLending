using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Logs
{
    public interface ILog
    {
        Task<Guid> AddLogs(LogModel logModel);
        Task UpdateLogs(LogModel logModel);
    }
}
