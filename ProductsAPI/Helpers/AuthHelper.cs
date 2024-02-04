using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using ProductsAPI.Data;

namespace ProductsAPI.Helpers;


public class AuthHelper
{
    private readonly IConfiguration _config;

    public AuthHelper( IConfiguration config )
    {
        _config = config;
    }

    public string CreateToken(int userId)
    {
        Claim[] claims = new Claim[]
        {
            new Claim("userId", userId.ToString())
        };
        
        string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;

        SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(tokenKeyString != null ? tokenKeyString : "")
        );

        SigningCredentials credentials = new SigningCredentials(
            tokenKey , SecurityAlgorithms.HmacSha512Signature
        ); 
        
        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity( claims ),
            SigningCredentials = credentials,
            Expires = DateTime.Now.AddDays( 1 )
        };

        JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

        SecurityToken token = TokenHandler.CreateToken(descriptor);

        return TokenHandler.WriteToken(token) ;
    }

    public byte[] GetPasswordHash(string password, byte[] passwordSalt)
    {
        string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value +
                                        Convert.ToBase64String(passwordSalt);

        return KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000000,
            numBytesRequested: 256 / 8);
    }
}