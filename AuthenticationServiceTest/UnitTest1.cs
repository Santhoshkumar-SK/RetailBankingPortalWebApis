using AuthenticationService;
using AuthenticationService.Controllers;
using AuthenticationService.Models;
using AuthenticationService.Repository;
using AuthenticationService.Data;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace AuthenticationServiceTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsTokenNotNullIsTokenNotNull_When_ValidUserCredentialsAreUsed_Employee()
        {
            Mock<IAuthenticationRepo> config = new Mock<IAuthenticationRepo>();
           AuthenticationController TokenObj = new AuthenticationController((IAuthenticationRepo)config.Object);
            var Result = TokenObj.login(new  UserDTO{ UserId = 1, Password = "1234", Role = "Employee" });
            Assert.IsNotNull(Result);
        }

        [Test]
        public void IsTokenNotNullIsTokenNotNull_When_ValidUserCredentialsAreUsed_Customer()
        {
            Mock<IAuthenticationRepo> config = new Mock<IAuthenticationRepo>();
            AuthenticationController TokenObj = new AuthenticationController((IAuthenticationRepo)config.Object);
            var Result = TokenObj.login(new UserDTO { UserId = 1, Password = "1234", Role = "Customer" });
            Assert.IsNotNull(Result);
        }

        [Test]
        public void IsTokenNull_When_InvalidUserCredentialsAreUsed()
        {
            Mock<IAuthenticationRepo> config = new Mock<IAuthenticationRepo>();
            var TokenObj = new AuthenticationController((IAuthenticationRepo)config.Object);
            var Result = TokenObj.login(new UserDTO() { UserId = 0, Password = "wronginput", Role = "wronginput" });
            Assert.IsNull(Result);
        }
    }
}