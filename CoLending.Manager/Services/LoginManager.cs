using CoLending.Core.RequestModels;
using CoLending.Core.ResponseModels;
using CoLending.Infrastructure.Interfaces;
using CoLending.Manager.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Manager.Services
{
    public class LoginManager : ILoginManager
    {
        private readonly ILoginRepository _loginRepository;

        public LoginManager(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }


        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            LoginResponse employeeLoginResponse = new LoginResponse();
            // will call sso api here
            employeeLoginResponse = await _loginRepository.Login(loginRequest);
            //employeeLoginResponse.BPNumber = employeeMasterResponseModel.BPNumber;
            //employeeLoginResponse.UserName = employeeMasterResponseModel.UserName;
            //employeeLoginResponse.FirstName = employeeMasterResponseModel.FirstName;
            //employeeLoginResponse.MiddleName = employeeMasterResponseModel.MiddleName;
            //employeeLoginResponse.LastName = employeeMasterResponseModel.LastName;
            //employeeLoginResponse.MobileNo = employeeMasterResponseModel.MobileNo;
            //employeeLoginResponse.EmailID = employeeMasterResponseModel.EmailID;
            //employeeLoginResponse.Role = employeeMasterResponseModel.DefaultRole;
            //employeeLoginResponse.RoleId = employeeMasterResponseModel.RoleId;
            //employeeLoginResponse.IsMultiRole = employeeMasterResponseModel.IsMultiRole;  // add by sumit
            //employeeLoginResponse.IsRpeortAccess = employeeMasterResponseModel.IsRpeortAccess;  // add by sumit


            return employeeLoginResponse;
        }

    }
}
