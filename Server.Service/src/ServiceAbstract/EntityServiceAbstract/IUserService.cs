using Server.Core.src.Common;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.EntityServiceAbstract
{
    public interface IUserService
    {
        Task<UserReadDto> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync(QueryOptions options);
        Task<UserReadDto> UpdateUserByIdAsync(Guid userId, UserUpdateDto user);
        Task<bool> DeleteUserByIdAsync(Guid id);
        Task<UserReadDto> CreateCustomerAsync(UserCreateDto user);
        // Task<UserReadDto> CreateAdminAsync(UserCreateDto user);
        Task<bool> CheckEmailAsync(string email);
        Task<bool> ChangePassword(Guid id, string password);
    }
}