using Moq;
using Xunit;
using AutoMapper;
using Server.Core.src.Entity;
using Server.Service.src.DTO;
using Server.Service.src.ServiceImplement.AuthServiceImplement;
using Server.Service.src.Shared;
using System.Threading.Tasks;
using Server.Core.src.RepoAbstract;
using System;
using Server.Service.src.ServiceAbstract.AuthServiceAbstract;
using Server.Core.src.Common;

namespace Server.Service.Tests.ServiceImplement.AuthServiceImplement
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepo> _mockUserRepo;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IPasswordService> _mockPwdService;
        private readonly Mock<IMapper> _mockMapper;

        public AuthServiceTests()
        {
            _mockUserRepo = new Mock<IUserRepo>();
            _mockTokenService = new Mock<ITokenService>();
            _mockPwdService = new Mock<IPasswordService>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task LoginAsync_ValidCredential_ReturnsToken()
        {
            // Arrange
            var authService = new AuthService(_mockUserRepo.Object, _mockTokenService.Object, _mockMapper.Object, _mockPwdService.Object);
            var credential = new UserCredential("test@example.com", "password123");
            var user = new User("username", credential.Email, "hashedPassword");
            var token = "token123";

            _mockUserRepo.Setup(repo => repo.GetUserByEmailAsync(credential.Email)).ReturnsAsync(user);
            _mockPwdService.Setup(pwdService => pwdService.VerifyPassword(credential.Password, user.Password, user.Salt)).Returns(true);
            _mockTokenService.Setup(tokenService => tokenService.GetToken(user)).Returns(token);

            // Act
            var result = await authService.LoginAsync(credential);

            // Assert
            Assert.Equal(token, result);
        }

        [Fact]
        public async Task LoginAsync_InvalidEmail_ThrowsNotFoundException()
        {
            // Arrange
            var authService = new AuthService(_mockUserRepo.Object, _mockTokenService.Object, _mockMapper.Object, _mockPwdService.Object);
            var credential = new UserCredential("nonexistent@example.com","password123");
            var expectedMessage = "Email hasn't been registered.";

            _mockUserRepo.Setup(repo => repo.GetUserByEmailAsync(credential.Email)).ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => authService.LoginAsync(credential));
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_ThrowsUnauthorizedException()
        {
            // Arrange
            var authService = new AuthService(_mockUserRepo.Object, _mockTokenService.Object, _mockMapper.Object, _mockPwdService.Object);
            var credential = new UserCredential("test@example.com", "password123");
            var user = new User("username", credential.Email, "hashedPassword");
            var expectedMessage = "Password incorrect.";

            _mockUserRepo.Setup(repo => repo.GetUserByEmailAsync(credential.Email)).ReturnsAsync(user);
            _mockPwdService.Setup(pwdService => pwdService.VerifyPassword(credential.Password, user.Password, user.Salt)).Returns(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<CustomException>(() => authService.LoginAsync(credential));
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
