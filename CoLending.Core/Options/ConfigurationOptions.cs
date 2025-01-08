using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Core.Options
{
    public class ConfigurationOptions
    {
        public const string Configuration = "Configuration";
        public int? EncryptDecryptEnable { get; set; }
        public string EncryptionKey { get; set; } = string.Empty;
        public string ReqEncryptKey { get; set; } = string.Empty;
        public string ReqEncryptIV { get; set; } = string.Empty;
        public int IsPIIRequired { get; set; }
        public int ADAuthEnable { get; set; }
        public int SwaggerEnable { get; set; } = 0;
        public string VaptSetting { get; set; } = string.Empty;
    }
}
