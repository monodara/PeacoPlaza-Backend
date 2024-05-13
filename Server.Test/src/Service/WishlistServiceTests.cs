using Moq;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Infrastructure.src.Database;
using Server.Service.src.DTO;
using Server.Service.src.ServiceImplement.EntityServiceImplement;

namespace Server.Test.src.Service
{
    public class WishlistServiceTests
    {
        private readonly User user = SeedingData.GetUsers()[0];
        [Fact]
        public async Task CreateWishlistAsync_ValidWishlist_ReturnsWishlistReadDto()
        {
            // Arrange
            var wishlistCreateDto = new WishlistCreateDto("Test Wishlist"); // Provide valid wishlist data
            var wishlistToAdd = wishlistCreateDto.CreateWishlist(user.Id);
            var mockWishlistRepo = new Mock<IWishlistRepo>();
            mockWishlistRepo.Setup(repo => repo.CreateWishlistAsync(It.IsAny<Wishlist>())).ReturnsAsync(wishlistToAdd);
            var wishlistService = new WishlistService(mockWishlistRepo.Object);

            // Act
            var result = await wishlistService.CreateWishlistAsync(user.Id, wishlistCreateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(wishlistCreateDto.Name, result.Name);
        }
        [Fact]
        public async Task GetWishlistByIdAsync_InvalidId_ThrowsResourceNotFoundException()
        {
            // Arrange
            var invalidWishlistId = Guid.NewGuid(); // Provide an invalid wishlist ID
            var mockWishlistRepo = new Mock<IWishlistRepo>();
            mockWishlistRepo.Setup(repo => repo.GetWishlistByIdAsync(invalidWishlistId)).ReturnsAsync((Wishlist)null); // Simulate wishlist not found
            var wishlistService = new WishlistService(mockWishlistRepo.Object);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => wishlistService.GetWishlistByIdAsync(invalidWishlistId));
        }
        [Fact]
        public async Task GetWishlistByUserAsync_ValidUser_ReturnsListOfWishlistReadDto()
        {
            // Arrange
            var wishlists = new List<Wishlist>
                {
                    new Wishlist("Wishlist 1", user.Id),
                    new Wishlist("Wishlist 2", user.Id)
                };
            var expectedWishlistDtos = wishlists.Select(wl => new WishlistReadDto().Transform(wl));
            var mockWishlistRepo = new Mock<IWishlistRepo>();
            mockWishlistRepo.Setup(repo => repo.GetWishlistByUserAsync(user.Id)).ReturnsAsync(wishlists);
            var wishlistService = new WishlistService(mockWishlistRepo.Object);

            // Act
            var result = await wishlistService.GetWishlistByUserAsync(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedWishlistDtos.Count(), result.Count());
            foreach (var expectedWishlistDto in expectedWishlistDtos)
            {
                Assert.Contains(result, r => r.Name == expectedWishlistDto.Name);
            }
        }

        [Fact]
        public async Task UpdateWishlistByIdAsync_InvalidId_ThrowsResourceNotFoundException()
        {
            // Arrange
            var invalidWishlistId = Guid.NewGuid(); // Provide an invalid wishlist ID
            var invalidWishlistUpdateDto = new WishlistUpdateDto(invalidWishlistId, "Updated Wishlist Name"); // Provide updated wishlist data
            var mockWishlistRepo = new Mock<IWishlistRepo>();
            mockWishlistRepo.Setup(repo => repo.GetWishlistByIdAsync(invalidWishlistId)).ReturnsAsync((Wishlist)null); // Simulate wishlist not found
            var wishlistService = new WishlistService(mockWishlistRepo.Object);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => wishlistService.UpdateWishlistByIdAsync(invalidWishlistUpdateDto));
        }

        [Fact]
        public async Task DeleteWishlistByIdAsync_ValidId_DeletesWishlistAndReturnsTrue()
        {
            // Arrange
            var validWishlistId = Guid.NewGuid();
            var mockWishlistRepo = new Mock<IWishlistRepo>();
            mockWishlistRepo.Setup(repo => repo.DeleteWishlistByIdAsync(validWishlistId)).ReturnsAsync(true); // Simulate successful deletion
            var wishlistService = new WishlistService(mockWishlistRepo.Object);

            // Act
            var result = await wishlistService.DeleteWishlistByIdAsync(validWishlistId);

            // Assert
            Assert.True(result);
            mockWishlistRepo.Verify(repo => repo.DeleteWishlistByIdAsync(validWishlistId), Times.Once);
        }
        [Fact]
        public async Task DeleteProductFromWishlishAsync_ValidData_ReturnsTrue()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var wishlistId = Guid.NewGuid();
            var mockWishlistRepo = new Mock<IWishlistRepo>();
            mockWishlistRepo.Setup(repo => repo.DeleteProductFromWishlishAsync(productId, wishlistId)).ReturnsAsync(true); // Simulate successful deletion
            var wishlistService = new WishlistService(mockWishlistRepo.Object);

            // Act
            var result = await wishlistService.DeleteProductFromWishlishAsync(productId, wishlistId);

            // Assert
            Assert.True(result);
            mockWishlistRepo.Verify(repo => repo.DeleteProductFromWishlishAsync(productId, wishlistId), Times.Once);
        }

        [Fact]
        public async Task AddProductToWishlishAsync_ValidData_ReturnsTrue()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var wishlistId = Guid.NewGuid();
            var mockWishlistRepo = new Mock<IWishlistRepo>();
            mockWishlistRepo.Setup(repo => repo.AddProductToWishlishAsync(productId, wishlistId)).ReturnsAsync(true); // Simulate successful addition
            var wishlistService = new WishlistService(mockWishlistRepo.Object);

            // Act
            var result = await wishlistService.AddProductToWishlishAsync(productId, wishlistId);

            // Assert
            Assert.True(result);
            mockWishlistRepo.Verify(repo => repo.AddProductToWishlishAsync(productId, wishlistId), Times.Once);
        }



    }
}