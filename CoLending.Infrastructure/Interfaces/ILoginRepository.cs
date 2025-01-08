using CoLending.Core.RequestModels;
using CoLending.Core.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Infrastructure.Interfaces
{
    public interface ILoginRepository
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
    }
}
