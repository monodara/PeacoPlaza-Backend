using Moq;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.AuthServiceAbstract;
using Server.Service.src.ServiceImplement.EntityServiceImplement;
using Server.Service.src.Shared;
using Xunit;

namespace Server.Test.src.Service
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepo> _mockUserRepo;
        private readonly Mock<IPasswordService> _mockPwdService;
        private readonly UserService _userService;
        public UserServiceTests()
        {
            _mockUserRepo = new Mock<IUserRepo>();
            _mockPwdService = new Mock<IPasswordService>();
            _userService = new UserService(_mockUserRepo.Object, _mockPwdService.Object);
        }

        [Fact]
        public async Task CreateCustomerAsync_EmailAlreadyRegistered_ThrowsBadRequestException()
        {
            // Arrange
            var userCreateDto = new UserCreateDto("testuser", "test@gmail.com","password");
                
            _mockUserRepo.Setup(repo => repo.CheckEmailAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => _userService.CreateCustomerAsync(userCreateDto));
            Assert.Equal("Email has been registered. Maybe try to login...", exception.Message);

            _mockUserRepo.Verify(repo => repo.CheckEmailAsync(It.IsAny<string>()), Times.Once);
        }
        [Fact]
        public async Task CreateCustomerAsync_EmailAvailable_CreatesUser()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.CheckEmailAsync(It.IsAny<string>())).ReturnsAsync(true);
            mockUserRepo.Setup(repo => repo.CreateUserAsync(It.IsAny<User>())).ReturnsAsync(new User("JohnMiller", "john.miller@mail.com", "miller123"));
            var userService = new UserService(mockUserRepo.Object, _mockPwdService.Object);
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
            var userService = new UserService(mockUserRepo.Object, _mockPwdService.Object);

            // Act
            var result = await userService.DeleteUserByIdAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_UserNotExists_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.DeleteUserByIdAsync(userId)).ReturnsAsync(false);
            var userService = new UserService(mockUserRepo.Object, _mockPwdService.Object);

            // Act + Assert
            await Assert.ThrowsAsync<CustomException>(() => userService.DeleteUserByIdAsync(userId));
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsListOfUsers()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepo>();
            var users = new List<User> { new User("1", "1@mail.com", "miller123"), new User("2", "2@mail.com", "miller123"), new User("3", "3@mail.com", "miller123") };
            mockUserRepo.Setup(repo => repo.GetAllUsersAsync(It.IsAny<QueryOptions>())).ReturnsAsync(users);
            var userService = new UserService(mockUserRepo.Object, _mockPwdService.Object);

            // Act
            var result = await userService.GetAllUsersAsync(new QueryOptions());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count());
        }
        [Fact]
        public async Task GetUserByIdAsync_UserNotFound_ThrowsRException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);
            var userService = new UserService(mockUserRepo.Object, _mockPwdService.Object);

            // Act + Assert
            await Assert.ThrowsAsync<CustomException>(() => userService.GetUserByIdAsync(userId));
        }
        [Fact]
        public async Task GetUserByIdAsync_UserFound_ReturnsUserReadDto()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("1", "1@mail.com", "miller123"); // Create a user with the given ID
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            var userService = new UserService(mockUserRepo.Object, _mockPwdService.Object);

            // Act
            var result = await userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.UserName, result.UserName);
        }

        [Fact]
        public async Task UpdateUserByIdAsync_UserNotFound_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userUpdateDto = new UserUpdateDto("oldName"); // Create a user update DTO with the given ID
            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);
            var userService = new UserService(mockUserRepo.Object, _mockPwdService.Object);

            // Act + Assert
            await Assert.ThrowsAsync<CustomException>(() => userService.UpdateUserByIdAsync(userId,userUpdateDto));
        }

        [Fact]
        public async Task UpdateUserByIdAsync_UpdateSuccessful_ReturnsUserReadDto()
        {
            // Arrange
            var userToUpdate = new User("oldName", "1@mail.com", "miller123"); // Create a user to be updated with the given ID
            var userId = userToUpdate.Id;
            var userUpdateDto = new UserUpdateDto("newName"); // Create a user update DTO with the given ID

            var mockUserRepo = new Mock<IUserRepo>();
            mockUserRepo.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(userToUpdate);

            var updatedUser = userUpdateDto.UpdateUser(userToUpdate); // update the user 

            mockUserRepo.Setup(repo => repo.UpdateUserByIdAsync(It.IsAny<User>())).ReturnsAsync(updatedUser); // Simulate update success

            var userService = new UserService(mockUserRepo.Object, _mockPwdService.Object);

            // Act
            var result = await userService.UpdateUserByIdAsync(userId,userUpdateDto);

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
            var userService = new UserService(mockUserRepo.Object, _mockPwdService.Object);

            // Act + Assert
            await Assert.ThrowsAsync<CustomException>(() => userService.ChangePasswordAsync(userId, newPassword));
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
            mockUserRepo.Setup(repo => repo.ChangePasswordAsync(userId, It.IsAny<string>(), It.IsAny<byte[]>())).ReturnsAsync(false); // Simulate password change failure
            var userService = new UserService(mockUserRepo.Object, _mockPwdService.Object);

            // Act
            var result = await userService.ChangePasswordAsync(userId, newPassword);

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
            mockUserRepo.Setup(repo => repo.ChangePasswordAsync(userId, It.IsAny<string>(), It.IsAny<byte[]>())).ReturnsAsync(true); // Simulate successful password change
            var userService = new UserService(mockUserRepo.Object, _mockPwdService.Object);

            // Act
            var result = await userService.ChangePasswordAsync(userId, newPassword);

            // Assert
            // Verify that GetUserByIdAsync is invoked with the correct user ID
            mockUserRepo.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once);

            // Assert the result
            Assert.True(result);
        }

    }
}