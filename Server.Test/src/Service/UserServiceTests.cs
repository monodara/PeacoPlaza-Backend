using Moq;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Service.src.DTO;
using Server.Service.src.ServiceImplement.EntityServiceImplement;
using Xunit;

namespace Server.Test.src.Service
{
    public class UserServiceTests
    {
        [Fact]
        public async Task CreateCustomerAsync_EmailNotAvailable_ThrowsValidationException()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.CheckEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
            var userService = new UserService(mockUserRepo.Object);
            var userCreateDto = new Mock<UserCreateDto>("test", "test@example.com", "password");

            // Act + Assert
            await Assert.ThrowsAsync<ValidationException>(() => userService.CreateCustomerAsync(userCreateDto.Object));
        }

        [Fact]
        public async Task CreateCustomerAsync_EmailAvailable_CreatesUser()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.CheckEmailAsync(It.IsAny<string>())).ReturnsAsync(true);
            mockUserRepo.Setup(repo => repo.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(new User("JohnMiller", "john.miller@mail.com", "miller123"));
            var userService = new UserService(mockUserRepo.Object);
            var userCreateDto = new Mock<UserCreateDto>("JohnMiller", "john.miller@mail.com", "miller123");

            // Act
            var result = await userService.CreateCustomerAsync(userCreateDto.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("john.miller@mail.com", result.Email);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_UserExists_DeletesUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.DeleteUserByIdAsync(userId)).ReturnsAsync(true);
            var userService = new UserService(mockUserRepo.Object);

            // Act
            var result = await userService.DeleteUserByIdAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_UserNotExists_ThrowsResourceNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.DeleteUserByIdAsync(userId)).ReturnsAsync(false);
            var userService = new UserService(mockUserRepo.Object);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => userService.DeleteUserByIdAsync(userId));
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsListOfUsers()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepo>();
            var users = new List<User> { new User("1", "1@mail.com", "miller123"), new User("2", "2@mail.com", "miller123"), new User("3", "3@mail.com", "miller123") };
            mockUserRepo.Setup(repo => repo.GetAllUsersAsync(It.IsAny<QueryOptions>())).ReturnsAsync(users);
            var userService = new UserService(mockUserRepo.Object);

            // Act
            var result = await userService.GetAllUsersAsync(new QueryOptions());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count());
        }
        [Fact]
        public async Task GetUserByIdAsync_UserNotFound_ThrowsResourceNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);
            var userService = new UserService(mockUserRepo.Object);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => userService.GetUserByIdAsync(userId));
        }
        [Fact]
        public async Task GetUserByIdAsync_UserFound_ReturnsUserReadDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("1", "1@mail.com", "miller123"); // Create a user with the given ID
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            var userService = new UserService(mockUserRepo.Object);

            // Act
            var result = await userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.UserName, result.UserName);
        }

        [Fact]
        public async Task UpdateUserByIdAsync_UserNotFound_ThrowsResourceNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userUpdateDto = new UserUpdateDto(userId, "oldName"); // Create a user update DTO with the given ID
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);
            var userService = new UserService(mockUserRepo.Object);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => userService.UpdateUserByIdAsync(userUpdateDto));
        }

        [Fact]
        public async Task UpdateUserByIdAsync_UpdateSuccessful_ReturnsUserReadDto()
        {
            // Arrange
            var userToUpdate = new User("oldName", "1@mail.com", "miller123"); // Create a user to be updated with the given ID
            var userId = userToUpdate.Id;
            var userUpdateDto = new UserUpdateDto(userId, "newName"); // Create a user update DTO with the given ID

            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(userToUpdate);

            var updatedUser = userUpdateDto.UpdateUser(userToUpdate); // update the user 

            mockUserRepo.Setup(repo => repo.UpdateUserByIdAsync(It.IsAny<User>())).ReturnsAsync(updatedUser); // Simulate update success

            var userService = new UserService(mockUserRepo.Object);

            // Act
            var result = await userService.UpdateUserByIdAsync(userUpdateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userUpdateDto.UserName, result.UserName);
        }

        [Fact]
        public async Task ChangePassword_UserNotFound_ThrowsResourceNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newPassword = "newPassword123";
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null); // Simulate user not found
            var userService = new UserService(mockUserRepo.Object);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => userService.ChangePassword(userId, newPassword));
        }
        [Fact]
        public async Task ChangePassword_ChangeFailed_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newPassword = "newPassword123";
            var userToUpdate = new User("oldName", "1@mail.com", "miller123");
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(userToUpdate);
            mockUserRepo.Setup(repo => repo.ChangePasswordAsync(userId, newPassword)).ReturnsAsync(false); // Simulate password change failure
            var userService = new UserService(mockUserRepo.Object);

            // Act
            var result = await userService.ChangePassword(userId, newPassword);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task ChangePassword_ValidIdAndPassword_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var newPassword = "newPassword123";
            var userToUpdate = new User("oldName", "1@mail.com", "miller123");
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(userToUpdate);
            mockUserRepo.Setup(repo => repo.ChangePasswordAsync(userId, newPassword)).ReturnsAsync(true); // Simulate successful password change
            var userService = new UserService(mockUserRepo.Object);

            // Act
            var result = await userService.ChangePassword(userId, newPassword);

            // Assert
            // Verify that GetUserByIdAsync is invoked with the correct user ID
            mockUserRepo.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);

            // Verify that ChangePasswordAsync is invoked with the correct user ID and password
            mockUserRepo.Verify(repo => repo.ChangePasswordAsync(userId, newPassword), Times.Once);

            // Assert the result
            Assert.True(result);
        }


    }
}