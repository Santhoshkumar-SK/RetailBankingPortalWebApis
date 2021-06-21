using AuthenticationService;
using AuthenticationService.Controllers;
using AuthenticationService.Models;
using AuthenticationService.Repository;
using AuthenticationService.Data;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationServiceTest
{
    public class Tests
    {
        private Mock<IAuthenticationRepo> _config;
        private AuthenticationController _TokenObj;

        [SetUp]
        public void Setup()
        {
            _config = new Mock<IAuthenticationRepo>();
            _TokenObj = new AuthenticationController(_config.Object);
        }

        [Test]
        public void IsTokenNotNullIsTokenNotNull_When_ValidUserCredentialsAreUsed_Employee()
        {
            //Arrange
            var Result = _TokenObj.login(new  UserDTO{ UserId = 1, Password = "pas@123", Role = "Employee" });
            
            //Assert
            Assert.IsNotNull(Result);
        }

        [Test]
        public void IsTokenNotNullIsTokenNotNull_When_ValidUserCredentialsAreUsed_Customer()
        {
            //Arrange
            var Result = _TokenObj.login(new UserDTO { UserId = 1, Password = "pas@123", Role = "Customer" });
            
            //Assert
            Assert.IsNotNull(Result);
        }

        [Test]
        public void IsTokenNull_When_InvalidUserCredentialsAreUsed()
        {
            //Arrange
            var Result = _TokenObj.login(new UserDTO() { UserId = 0, Password = "wronginput", Role = "wronginput" });
            
            //Assert
            Assert.IsNull(Result);
        }

        [Test]
        public void IsTokenNotNull_When_CreateCustomer_Valid_Details()
        {
            //Arrange
            var Result = _TokenObj.addCustomer(new UserDTO { UserId = 1, Password = "pas@123", Role = "Customer" });
            
            //Assert
            Assert.IsNotNull(Result);
        }

        [Test]
        public void IsTokenNull_When_CreateCustomer_InValid_Details()
        {
            //Arrange
            var Result = _TokenObj.addCustomer(new UserDTO { UserId = 0, Password = "Wronginput", Role = "wronginput" });
            
            //Assert
            Assert.IsNull(Result);
        }

        [Test]
        public void IsTokenNotNull_When_GenerateToken_Valid_ReturnsOk()
        {
           //Arrange
           var Result = _config.Setup(s => s.generateJSONWebToken(It.IsAny<UserDTO>())).Returns("True");
           
            //Assert
           Assert.IsNotNull(Result);
        }

        [Test]
        public void IsTokenNull_When_GenerateToken_InValid_ReturnsOk()
        {
            //Arrange
            var Result = _config.Setup(s => s.generateJSONWebToken(It.IsAny<UserDTO>())).Returns("False");
            
            //Assert
            Assert.IsNull(Result);
        }

        [Test]
        public void IsTokenNotNull_When_AuthenticateEmployee_Valid_ReturnOk()
        {
            //Arrange
            var Result = _config.Setup(s => s.authenticateEmployee(new UserDTO
            {
                UserId = 1,
                Password = "pas@123",
                Role = "Employee"
            }));

            //Assert
            Assert.IsNotNull(Result);
        }
        
        [Test]
        public void IsTokenNotNull_When_AuthenticateCustomer_Valid_ReturnOk()
        {
            //Arrange
            var Result = _config.Setup(s => s.authenticateCustomer(new UserDTO
            {
                UserId = 1,
                Password = "pas@123",
                Role = "Customer"
            }));

            //Assert
            Assert.IsNotNull(Result);
        }

        [Test]
        public void IsTokenNotNull_When_AuthenticateUser_Valid_ReturnOk()
        {
            //Arrange
            var Result = _config.Setup(s => s.authenticateUser(new UserDTO
            {
                UserId = 1,
                Password = "Pas@123",
                Role = "Employee"
            }));

            //Assert
            Assert.IsNotNull(Result);
        }

        [Test]
        public void IsTokenNull_When_AuthenticateUser_InValid_ReturnOk()
        {
            //Arrange
            var Result = _config.Setup(s => s.authenticateUser(new UserDTO
            {
                UserId = 1,
                Password = "wronginput",
                Role = "wronginput"
            }));

            //Assert
            Assert.IsNull(Result);
        }

        [Test]
        public void IsTokenNotNull_When_addCustomer_Valid_ReturnOk()
        {
            //Arrange
            var Result = _config.Setup(s => s.addCustomer(new UserDTO
            {
                UserId = 1,
                Password = "pas@123",
                Role = "Customer"
            }));

            //Assert
            Assert.IsNotNull(Result);
        }

        [Test]
        public void IsTokenNull_When_addCustomer_InValid_ReturnOk()
        {
            //Arrange
            var Result = _config.Setup(s => s.addCustomer(new UserDTO
            {
                UserId = 1,
                Password = "wronginput",
                Role = "wronginput"
            }));

            //Assert
            Assert.IsNull(Result);
        }

    }
}