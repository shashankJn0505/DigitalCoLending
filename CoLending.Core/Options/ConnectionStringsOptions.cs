using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Core.Options
{
    public class ConnectionStringsOptions
    {
        public const string ConnectionStrings = "ConnectionStrings";
        public string DefaultConnection { get; set; } = string.Empty;
    }
}
