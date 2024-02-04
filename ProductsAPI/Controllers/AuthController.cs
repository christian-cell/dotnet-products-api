using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProductModels.Dtos;
using ProductModels.Models;
using ProductsAPI.Data;
using ProductsAPI.Helpers;
using Exception = System.Exception;

namespace ProductsAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]

public class AuthController : ControllerBase
{
    
    private ILogger<AuthController> _logger;
    private readonly AuthHelper _authelper;
    private readonly DataContextDapper _dapper;
    private readonly IConfiguration _config;
    

    public AuthController(
        AuthHelper authelper,
        IConfiguration config,
        ILogger<AuthController> logger
        )
    {
        _logger = _logger;
        _dapper = new DataContextDapper( config );
        _config = config;
        _authelper = authelper;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlCheckUserExists = "SELECT Email FROM ProductsSchema.Auth WHERE Email = '" +
                    userForRegistration.Email + "'";

                IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckUserExists);
                if (existingUsers.Count() == 0)
                {
                    byte[] passwordSalt = new byte[128 / 8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    byte[] passwordHash = _authelper.GetPasswordHash(userForRegistration.Password, passwordSalt);

                    string sqlAddAuth = @"
                        INSERT INTO ProductsSchema.Auth  ([Email],
                        [PasswordHash],
                        [PasswordSalt]) VALUES ('" + userForRegistration.Email +
                        "', @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;

                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    sqlParameters.Add(passwordSaltParameter);
                    sqlParameters.Add(passwordHashParameter);

                    if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                    {
                        
                        string sqlAddUser = @"
                            INSERT INTO ProductsSchema.Users(
                                [FirstName],
                                [LastName],
                                [Email],
                                [Gender],
                                [Active]
                            ) VALUES (" +
                                "'" + userForRegistration.FirstName + 
                                "', '" + userForRegistration.LastName +
                                "', '" + userForRegistration.Email + 
                                "', '" + userForRegistration.Gender + 
                                "', 1)";
                        if (_dapper.ExecuteSql(sqlAddUser))
                        {
                            return Ok();
                        }
                        throw new Exception("Failed to add user.");
                    }
                    throw new Exception("Failed to register user.");
                }
                throw new Exception("User with this email already exists!");
            }
            throw new Exception("Passwords do not match!");
        }
    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(UserForLoginDto userForLogin)
    {

        string sqlForHashAndSalt = @"SELECT 
            [PasswordSalt] , [PasswordHash] FROM ProductsSchema.Auth WHERE Email = '" +
                                   userForLogin.Email + "'";

        UserForLoginConfirmationDto userForLoginConfirmation =
            _dapper.LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);

        if (userForLoginConfirmation != null)
        {
            byte[] passwordHash = _authelper.GetPasswordHash(userForLogin.Password, userForLoginConfirmation.PasswordSalt);

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != userForLoginConfirmation.PasswordHash[i])
                {
                    return StatusCode(401, "Password Incorrecto");
                }
            }

            string userIdSql = @"
               SELECT UserId FROM ProductsSchema.Users WHERE Email = '" +
                               userForLogin.Email + "'";
            
            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string>
                {
                    { "token", _authelper.CreateToken(userId) }
                }
            );
        }
        else
        {
            throw new Exception("Usuario no encontrado en Auth");
        }
    }

    
}