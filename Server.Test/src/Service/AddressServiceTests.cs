using Moq;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Infrastructure.src.Database;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;
using Server.Service.src.ServiceImplement.EntityServiceImplement;

namespace Server.Test.src.Service
{
    public class AddressServiceTests
    {
        private readonly User user = SeedingData.GetUsers()[0];
        [Fact]
        public async Task CreateAddressAsync_ValidAddress_ReturnsAddressReadDto()
        {
            // Arrange
            var addressCreateDto = new AddressCreateDto("41C", "Asemakatu", "Pori", "Finland", "61200", "4198767000", "John", "Mull", "K-market");
            var addressToAdd = addressCreateDto.CreateAddress(user.Id); // Mock or create an actual instance of Address
            var mockAddressRepo = new Mock<IAddressRepo>();
            mockAddressRepo.Setup(repo => repo.CreateAddressAsync(It.IsAny<Address>())).ReturnsAsync(addressToAdd);
            var addressService = new AddressService(mockAddressRepo.Object);

            // Act
            var result = await addressService.CreateAddressAsync(user.Id, addressCreateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(addressToAdd.Postcode, result.Postcode);
        }


        [Fact]
        public async Task UpdateAddressByIdAsync_AddressNotFound_ThrowsResourceNotFoundException()
        {
            // Arrange
            var addressToUpdate = new Address("41C", "Asemakatu", "Pori", "Finland", "61200", "4198767000", "John", "Mull", "K-market", user.Id);
            var addressId = addressToUpdate.Id;
            var addressUpdateDto = new AddressUpdateDto("41C", "Asemakatu", "Pori", "Finland", "61200", "4198767000", "John", "Mull", "K-market");
            var mockAddressRepo = new Mock<IAddressRepo>();
            mockAddressRepo.Setup(repo => repo.GetAddressByIdAsync(addressId)).ReturnsAsync((Address)null);
            var addressService = new AddressService(mockAddressRepo.Object);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => addressService.UpdateAddressByIdAsync(addressId, addressUpdateDto));
        }

        [Fact]
        public async Task DeleteAddressByIdAsync_AddressNotFound_ThrowsResourceNotFoundException()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var mockAddressRepo = new Mock<IAddressRepo>();
            mockAddressRepo.Setup(repo => repo.DeleteAddressByIdAsync(addressId)).ReturnsAsync(false); // Simulate delete failure
            var addressService = new AddressService(mockAddressRepo.Object);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => addressService.DeleteAddressByIdAsync(addressId));
        }

        [Fact]
        public async Task GetDefaultAddressAsync_UserHasNoDefaultAddress_ThrowsResourceNotFoundException()
        {
            // Arrange
            var mockAddressRepo = new Mock<IAddressRepo>();
            var userService = new AddressService(mockAddressRepo.Object);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => userService.GetDefaultAddressAsync(user.Id));
        }
        [Fact]
        public async Task GetDefaultAddressAsync_UserHasDefaultAddress_ReturnsAddressReadDto()
        {
            // Arrange
            var defaultAddress = new Address("41C", "Asemakatu", "Pori", "Finland", "61200", "4198767000", "John", "Mull", "K-market", user.Id);
            var mockAddressRepo = new Mock<IAddressRepo>();
            mockAddressRepo.Setup(repo => repo.GetDefaultAddressAsync(user.Id)).ReturnsAsync(defaultAddress);

            user.DefaultAddressId = defaultAddress.Id; // Set default address ID for the user
            var addressService = new AddressService(mockAddressRepo.Object);
            // Act
            var result = await addressService.GetDefaultAddressAsync(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(defaultAddress.Postcode, result.Postcode);
        }


        [Fact]
        public async Task SetDefaultAddressAsync_SettingDefaultAddressFails_ThrowsInvalidOperationException()
        {
            // Arrange
            var addressId = Guid.NewGuid();
            var mockAddressRepo = new Mock<IAddressRepo>();
            mockAddressRepo.Setup(repo => repo.SetDefaultAddressAsync(It.IsAny<Guid>(), addressId)).ReturnsAsync(false); // Simulate setting default address failure
            var addressService = new AddressService(mockAddressRepo.Object);

            // Act + Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => addressService.SetDefaultAddressAsync(user.Id, addressId));
        }
        [Fact]
        public async Task SetDefaultAddressAsync_ValidAddressId_ReturnsTrue()
        {
            // Arrange
            var addressId = Guid.NewGuid(); // Provide a valid address ID
            var mockAddressRepo = new Mock<IAddressRepo>();
            mockAddressRepo.Setup(repo => repo.SetDefaultAddressAsync(It.IsAny<Guid>(), addressId)).ReturnsAsync(true); // Simulate successful setting of default address
            var addressService = new AddressService(mockAddressRepo.Object);

            // Act
            var result = await addressService.SetDefaultAddressAsync(user.Id, addressId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAddressByIdAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var addressId = Guid.NewGuid(); // Provide a valid address ID
            var mockAddressRepo = new Mock<IAddressRepo>();
            mockAddressRepo.Setup(repo => repo.DeleteAddressByIdAsync(addressId)).ReturnsAsync(true); // Simulate successful deletion
            var addressService = new AddressService(mockAddressRepo.Object);

            // Act
            var result = await addressService.DeleteAddressByIdAsync(addressId);

            // Assert
            Assert.True(result);
        }
        [Fact]
        public async Task DeleteAddressByIdAsync_InvalidId_ThrowsResourceNotFoundException()
        {
            // Arrange
            var invalidAddressId = Guid.NewGuid(); // Provide an invalid address ID
            var mockAddressRepo = new Mock<IAddressRepo>();
            mockAddressRepo.Setup(repo => repo.DeleteAddressByIdAsync(invalidAddressId)).ReturnsAsync(false); // Simulate deletion failure
            var addressService = new AddressService(mockAddressRepo.Object);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => addressService.DeleteAddressByIdAsync(invalidAddressId));
        }
        [Fact]
        public async Task GetAddressByIdAsync_ValidId_ReturnsAddressReadDto()
        {
            // Arrange
            var addressId = Guid.NewGuid(); // Provide a valid address ID
            var address = new Address("41C", "Asemakatu", "Pori", "Finland", "61200", "4198767000", "John", "Mull", "K-market", user.Id);
            var mockAddressRepo = new Mock<IAddressRepo>();
            mockAddressRepo.Setup(repo => repo.GetAddressByIdAsync(addressId)).ReturnsAsync(address);
            var addressService = new AddressService(mockAddressRepo.Object);

            // Act
            var result = await addressService.GetAddressByIdAsync(addressId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(address.Postcode, result.Postcode);
        }
        [Fact]
        public async Task GetAddressByIdAsync_InvalidId_ThrowsResourceNotFoundException()
        {
            // Arrange
            var invalidAddressId = Guid.NewGuid(); // Provide an invalid address ID
            var mockAddressRepo = new Mock<IAddressRepo>();
            mockAddressRepo.Setup(repo => repo.GetAddressByIdAsync(invalidAddressId)).ReturnsAsync((Address)null); // Simulate address not found
            var addressService = new AddressService(mockAddressRepo.Object);

            // Act + Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(() => addressService.GetAddressByIdAsync(invalidAddressId));
        }


    }
}