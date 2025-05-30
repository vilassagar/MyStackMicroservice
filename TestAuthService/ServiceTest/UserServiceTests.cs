using AuthService.Dto;
using AuthService.Model;
using AuthService.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAuthService.Base;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestAuthService.ServiceTest
{
    [TestClass]
    public class UserServiceTests : DatabaseTestBase
    {
        private IUserService _userService;

        [TestInitialize]
        public void Setup()
        {
            _userService = _serviceProvider.GetRequiredService<IUserService>();
        }

        [TestMethod]
        public async Task CreateAsync_ValidUser_SimulatesCreationWithoutCommit()
        {
            // Arrange
            var createDto = new CreateUserDto
            {
                FirstName = "Service",
                LastName = "Test",
                Email = "service.test@test.com",
                UserName = "service.test",
                Password = "TestPass123!",
                Address = "Service Test Address"
            };

            // Act
            var result = await _userService.CreateAsync(createDto);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("Service", result.Data.FirstName);

            // User creation was simulated but not committed to database
            // because we're in testing environment
        }

        [TestMethod]
        public async Task GetPagedAsync_ReturnsTestData()
        {
            // Arrange
            var parameters = new PaginationParameters { PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _userService.GetPagedAsync(parameters);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Data.Any());

            // Should include our seeded test users
            var users = result.Data.Data.ToList();
            Assert.IsTrue(users.Any(u => u.Email.Contains("@test.com")));
        }
    }
}
