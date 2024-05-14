using Server.Core.src.Entity;
using Server.Core.src.Common;

namespace Server.Core.src.RepoAbstract
{
    public interface IUserRepo
    {
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync(QueryOptions options);
        Task<User> UpdateUserByIdAsync(User user);
        Task<bool> DeleteUserByIdAsync(Guid id);
        Task<User> CreateUserAsync(User user);
        Task<bool> ChangePasswordAsync(Guid userId, string newPassword, byte[] salt);
        Task<bool> CheckEmailAsync(string email);
        Task<User> GetUserByCredentialsAsync(UserCredential userCredential);

    }
}