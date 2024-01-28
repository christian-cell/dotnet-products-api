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
                string sqlCheckUserExists = "SELECT Email FROM DotnetProductsPractice.Auth WHERE Email = '" +
                    userForRegistration.Email + "'";

                IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckUserExists);
                if (existingUsers.Count() == 0)
                {
                    byte[] passwordSalt = new byte[128 / 8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    byte[] passwordHash = GetPasswordHash(userForRegistration.Password, passwordSalt);

                    string sqlAddAuth = @"
                        INSERT INTO DotnetProductsPractice.Auth  ([Email],
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
                            INSERT INTO DotnetProductsPractice.Users(
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
                [PasswordHash],
                [PasswordSalt] FROM DotnetProductsPractice.Auth WHERE Email = '" +
                                   userForLogin.Email + "'";

        UserForLoginConfirmationDto userForConfirmation = _dapper
            .LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);

        if (userForConfirmation != null)
        {
            byte[] passwordHash = GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);


            for (int index = 0; index < passwordHash.Length; index++)
            {
                if (passwordHash[index] != userForConfirmation.PasswordHash[index]){
                    return StatusCode(401, "Incorrect password!");
                }
            }

            string userIdSql = @"
                SELECT UserId FROM DotnetProductsPractice.Users WHERE Email = '" +
                               userForLogin.Email + "'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string>
            {
                {"token" , _authelper.CreateToken(userId)}
            }); 
        }
        else
        {
            throw new Exception("usuario no encontrado en base de datos");
        }

        
    }

    private byte[] GetPasswordHash(string password, byte[] passwordSalt)
    {
        string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value +
                                        Convert.ToBase64String(passwordSalt);

        return KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 1000000,
            numBytesRequested: 256 / 8
        );
    }
    
    [HttpGet("RefreshToken")]
    public string RefreshToken()
    {
        string userIdSql = @"
                SELECT UserId FROM DotnetProductsPractice.Users WHERE UserId = '" +
                           User.FindFirst("userId")?.Value + "'";

        int userId = _dapper.LoadDataSingle<int>(userIdSql);
        return _authelper.CreateToken(userId);

    }
}