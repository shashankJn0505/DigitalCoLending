﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Core.RequestModels
{
    public class LoginRequest
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
