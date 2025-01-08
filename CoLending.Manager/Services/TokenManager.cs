using CoLending.Core.Options;
using CoLending.Core.ResponseModels;
using CoLending.Manager.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoLending.Manager.Services
{
    public class TokenManager : ITokenManager
    {
        private readonly TokenOptions _tokenOptions;

        public TokenManager(IOptions<TokenOptions> tokenOptions)
        {
            _tokenOptions = tokenOptions.Value;
        }
        public string AccesToken(TokenClaims claimparms)
        {
            var securityKey = new SymmetricSecurityKey(
           Encoding.ASCII.GetBytes(_tokenOptions.Secret));
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);
            var accessTokenExpiration = DateTime.UtcNow.AddHours(_tokenOptions.AccessTokenExpiration);
            var jwtSecurityToken = new JwtSecurityToken(issuer: _tokenOptions.Issuer,
                                audience: _tokenOptions.Audience,
                                claims: GetClaims(claimparms),
                                expires: accessTokenExpiration,
                                notBefore: DateTime.UtcNow,
                                signingCredentials: signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
               .WriteToken(jwtSecurityToken);
            return tokenToReturn;
        }

        private IEnumerable<Claim> GetClaims(TokenClaims _claimparms)
        {
            var claimsForToken = new List<Claim>
        {
            new Claim("UserId", _claimparms.UserId.ToString()),
        };

            return claimsForToken;
        }
    }
}
