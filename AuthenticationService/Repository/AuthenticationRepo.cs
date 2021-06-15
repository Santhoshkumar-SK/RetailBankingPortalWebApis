using AuthenticationService.Data;
using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Repository
{
    public class AuthenticationRepo : IAuthenticationRepo
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AuthenticationRepo));

        AuthenticationContext _context;
        IConfiguration _config;
        public AuthenticationRepo(AuthenticationContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public string generateJSONWebToken(UserDTO userInfo)
        {
            _log4net.Info("Generating Token Started..");
            string token = null;

            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


                var tokenDescripter = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                         new Claim(ClaimTypes.Role, userInfo.Role),
                        new Claim("UserID", userInfo.UserId.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = credentials
                };

                var tokenHandeler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandeler.CreateToken(tokenDescripter);
                token = tokenHandeler.WriteToken(securityToken);
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                return null;
            }
           
            return token;

        }

        public async Task<Boolean> authenticateUser(UserDTO user)
        {


            if (user.Role == "Employee")
            {
                return authenticateEmployee(user);
            }

            return await authenticateCustomer(user);
        }

        public async Task<Boolean> authenticateCustomer(UserDTO user)
        {
            
            try
            {
                User userFromDB = await _context.Authentication.Where(userid => userid.UserId == user.UserId && userid.Role == user.Role).FirstOrDefaultAsync();

                if (userFromDB != null)
                {
                    var hmac = new HMACSHA512(userFromDB.PasswordSalt);
                    var userPass = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

                    for (int i = 0; i < userPass.Length; i++)
                    {
                        if (userPass[i] != userFromDB.PasswordHash[i])
                        {
                            _log4net.Info("Invalid Password");
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                _log4net.Error(exception.Message);
                return false;
            }


            _log4net.Info("User Validated Successfully");
            return true;
        }

        public  bool authenticateEmployee(UserDTO user)
        {
            List<UserDTO> employeeList = new List<UserDTO>
            {
                new UserDTO{UserId=1,Password="1234",Role="Employee"},
                new UserDTO{UserId=2,Password="12345",Role="Employee"}
            };

            UserDTO result = employeeList.Find(userFormList => userFormList.UserId == user.UserId && userFormList.Password == user.Password);

            if (result != null)
                return true;
            return false;
        }

        public async  Task<Boolean> addCustomer(UserDTO user)
        {
            try
            {
                User userForDB = generateUserForDB(user);

                _context.Add(userForDB);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception exception)
            {
                _log4net.Info(exception.Message);
                return false;
            }
        }

        public User generateUserForDB(UserDTO user)
        {
            var hmac = new HMACSHA512();

            return new User()
            {
                UserId = user.UserId,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password)),
                PasswordSalt = hmac.Key,
                Role = user.Role
            };
        }

    }
}
