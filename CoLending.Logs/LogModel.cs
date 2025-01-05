using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Logs
{
    public class LogModel
    {
        public Guid Id { get; set; }
        public string BpNo { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string RequestHttpVerb { get; set; } = string.Empty;
        public string RequestBody { get; set; } = string.Empty;
        public string ResponseBody { get; set; } = string.Empty;
        public int? ResponseStatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
