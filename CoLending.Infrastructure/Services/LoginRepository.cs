using CoLending.Core.Models;
using CoLending.Core.Options;
using CoLending.Core.RequestModels;
using CoLending.Core.ResponseModels;
using CoLending.Infrastructure.HttpService;
using CoLending.Infrastructure.Interfaces;
using CoLending.Infrastructure.SqlService;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Infrastructure.Services
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IHttpService _httpService;
        private readonly ISqlUtility _sqlUtility;
        private readonly ConnectionStringsOptions _connectionStringsOptions;

        public LoginRepository(IHttpService httpService, ISqlUtility sqlUtility, IOptions<ConnectionStringsOptions> connectionStringsOptions)
        {
            _httpService = httpService;
            _sqlUtility = sqlUtility;
            _connectionStringsOptions = connectionStringsOptions.Value;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {


            LoginResponse Response = new LoginResponse();
            using (DigitalcolendingContext _context = new DigitalcolendingContext())
            {
                var data = _context.TblUserDetails.FirstOrDefault(x => x.UserId == loginRequest.UserId && x.Password == loginRequest.Password);
                if (data != null)
                {
                    Response.UserId = data.UserId;
                }
            }
            return Response;
        }
    }
}
