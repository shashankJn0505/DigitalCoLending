using CoLending.Core.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Manager.Interfaces
{
    public interface ITokenManager
    {
        string AccesToken(TokenClaims claimPram);
    }
}
