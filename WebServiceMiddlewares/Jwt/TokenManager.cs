using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySystem.Base.Extensions;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace vitaaid.com.Jwt
{
  public interface ITokenManager
    {
        public JwtToken CreateStandard(string userid, string username, string Jti, string appname);
        public JwtSecurityToken GetToken(HttpContext context);

    }
    public class TokenManager: ITokenManager
    {
        public virtual JwtOptions options { get; set; }
        public TokenManager(IOptions<JwtOptions> _options)
        {
            options = _options.Value;
        }
        public JwtToken CreateStandard(string userid, string username, string Jti, string appname)
        {
            // STEP1: create Claims，part of JWT Payload
            var userClaims = new ClaimsIdentity(new[] {
                new Claim(JwtRegisteredClaimNames.NameId, userid),
                new Claim(JwtRegisteredClaimNames.Name, username),
                new Claim(JwtRegisteredClaimNames.Sub, appname),
                new Claim(JwtRegisteredClaimNames.Jti, Jti)
            });
            // STEP2: create JWT TokenHandler and TokenDescriptor
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = options.Issuer,
                Audience = options.Audience,
                Subject = userClaims,
                Expires = DateTime.UtcNow.Add(options.ValidFor),
                SigningCredentials = options.SigningCredentials,
                NotBefore = options.NotBefore,
                IssuedAt = options.IssuedAt

            };
            //create JWT Token
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            // serializeToken JWT Token
            var serializeToken = tokenHandler.WriteToken(securityToken);
            return new JwtToken
            {
                access_token = serializeToken,
                refresh_token = Guid.NewGuid().ToString().Replace("-", ""),
                expires_in = options.ValidFor.TotalMinutes.ToInt()
            };
        }
        public JwtSecurityToken GetToken(HttpContext context)
        {
            //string token = context.Request.Headers["Authoriaztion"];
            context.Request.Headers.TryGetValue("Authorization", out var values);
            if (string.IsNullOrWhiteSpace(values))
                return null;

            if (values[0].StartsWith("Bearer"))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = values[0].Substring("Bearer".Length + 1);
                if (tokenHandler.CanReadToken(token))
                    return tokenHandler.ReadJwtToken(token);
            }
            return null;
        }


        public JwtToken Create(string userid, string username)
        {
            var exp = (int)options.ValidFor.TotalSeconds;
            var payload = new JwtPayload
            {
                UserID = userid,
                UserName = username,
                exp = Convert.ToInt32(
                    (DateTime.UtcNow.AddSeconds(exp) - new DateTime(1970, 1, 1)).TotalSeconds)
            };
            var json = JsonConvert.SerializeObject(payload);
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            var iv = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);

            var encrypt = JwtTokenCrypto.AESEncrypt(base64, options.SecretKey.Substring(0, 16), iv);
            var signature = JwtTokenCrypto.ComputeHMACSHA256(iv + "." + encrypt, options.SecretKey.Substring(0, 64));
            return new JwtToken
            {
                access_token = iv + "." + encrypt + "." + signature,
                refresh_token = Guid.NewGuid().ToString().Replace("-", ""),
                expires_in = exp
            };
        }

    }
}
