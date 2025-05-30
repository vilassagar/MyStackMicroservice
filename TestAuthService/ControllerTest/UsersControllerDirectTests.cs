using AuthService.Controllers;
using AuthService.Dto;
using AuthService.Model;
using Microsoft.AspNetCore.Mvc;
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

namespace TestAuthService.ControllerTest
{
    // Method 1: Direct Controller Testing (Unit Test Style)
    [TestClass]
    public class UsersControllerDirectTests : DatabaseTestBase
    {
        private UsersController _controller;
        private IUserService _userService;

        [TestInitialize]
        public void Setup()
        {
            _userService = _serviceProvider.GetRequiredService<IUserService>();
            _controller = new UsersController(_userService);
        }

        [TestMethod]
        public async Task GetUsers_DirectCall_ReturnsOkResult()
        {
            // Arrange
            var parameters = new PaginationParameters { PageNumber = 1, PageSize = 10 };

            // Act - Direct controller method call
            var result = await _controller.GetUsers(parameters);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            var pagedResult = okResult.Value as PagedResult<UserDto>;

            Assert.IsNotNull(pagedResult);
            Assert.IsTrue(pagedResult.Data.Any());
            Assert.IsTrue(pagedResult.TotalCount >= 3);
        }

        [TestMethod]
        public async Task GetUser_DirectCall_ExistingUser_ReturnsUser()
        {
            // Arrange
            // First, get a user ID from our seeded data
            var users = await _userService.GetPagedAsync(new PaginationParameters { PageNumber = 1, PageSize = 1 });
            var userId = users.Data.Data.First().Id;

            // Act - Direct controller method call
            var result = await _controller.GetUser(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            var user = okResult.Value as UserDto;

            Assert.IsNotNull(user);
            Assert.AreEqual(userId, user.Id);
        }

        [TestMethod]
        public async Task CreateUser_DirectCall_ValidData_ReturnsCreatedResult()
        {
            // Arrange
            var createDto = new CreateUserDto
            {
                FirstName = "Direct",
                LastName = "Test",
                Email = "direct.test@test.com",
                UserName = "direct.test",
                Password = "TestPass123!",
                Address = "Direct Test Address",
                RoleNames = new List<string> { "User" }
            };

            // Act - Direct controller method call
            var result = await _controller.CreateUser(createDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            var createdResult = result as CreatedAtActionResult;
            var user = createdResult.Value as UserDto;

            Assert.IsNotNull(user);
            Assert.AreEqual("Direct", user.FirstName);
            Assert.AreEqual("direct.test@test.com", user.Email);
        }

        [TestMethod]
        public async Task UpdateUser_DirectCall_ValidData_ReturnsOkResult()
        {
            // Arrange
            var users = await _userService.GetPagedAsync(new PaginationParameters { PageNumber = 1, PageSize = 1 });
            var userId = users.Data.Data.First().Id;

            var updateDto = new UpdateUserDto
            {
                FirstName = "Updated",
                LastName = "User",
                Email = "updated.user@test.com",
                Address = "Updated Address",
                RoleNames = new List<string> { "User" }
            };

            // Act - Direct controller method call
            var result = await _controller.UpdateUser(userId, updateDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            var user = okResult.Value as UserDto;

            Assert.IsNotNull(user);
            Assert.AreEqual("Updated", user.FirstName);
        }

        [TestMethod]
        public async Task SearchUsers_DirectCall_ReturnsFilteredResults()
        {
            // Arrange
            var searchTerm = "john";
            var parameters = new PaginationParameters { PageNumber = 1, PageSize = 10 };

            // Act - Direct controller method call
            var result = await _controller.SearchUsers(searchTerm, parameters);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            var pagedResult = okResult.Value as PagedResult<UserDto>;

            Assert.IsNotNull(pagedResult);
            Assert.IsTrue(pagedResult.Data.Any());
        }
    }
}
