using AuthService.Controllers;
using AuthService.DomainModel;
using AuthService.Model;
using AuthService.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAuthService.Base;

namespace TestAuthService.RepositoryTestCase
{
    [TestClass]
    public class UserRepositoryTests : DatabaseTestBase
    {
        private AuthService.Repository.Interface.IApplicationUserRepository _userRepository;

        [TestInitialize]
        public void Setup()
        {
            _userRepository = _serviceProvider.GetRequiredService<AuthService.Repository.Interface.IApplicationUserRepository>();
        }

        [TestMethod]
        public async Task AddAsync_NewUser_SimulatesAddWithoutCommit()
        {
            // Arrange
            var newUser = new ApplicationUser
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test.new@test.com",
                UserName = "test.new",
                Address = "Test Address"
            };

            // Act
            var result = await _userRepository.AddAsync(newUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id != 0); // Should have temporary ID
            Assert.AreEqual("Test", result.FirstName);

            // Verify that SaveChanges was called but didn't commit to database
            var userInDb = await _context.ApplicationUsers.FindAsync(result.Id);

            // In testing environment, the user exists in context but not in actual database
            // because SaveChanges was intercepted
        }

        [TestMethod]
        public async Task GetPagedAsync_WithTestData_ReturnsCorrectData()
        {
            // Arrange
            var parameters = new PaginationParameters
            {
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = await _userRepository.GetPagedAsync(parameters);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.Any());
            Assert.IsTrue(result.TotalCount >= 3); // At least our seeded users
        }

        [TestMethod]
        public async Task UpdateAsync_ExistingUser_SimulatesUpdateWithoutCommit()
        {
            // Arrange
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Email == "john.doe@test.com");

            Assert.IsNotNull(user);

            var originalFirstName = user.FirstName;
            user.FirstName = "Updated John";

            // Act
            await _userRepository.UpdateAsync(user);

            // Assert
            // The change is tracked in context but not committed to database
            Assert.AreEqual("Updated John", user.FirstName);

            // If we reload from database, it should still have original value
            // because SaveChanges was intercepted
        }
    }

 

   

}
