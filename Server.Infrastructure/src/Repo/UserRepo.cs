

using Microsoft.EntityFrameworkCore;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Core.src.ValueObject;
using Server.Infrastructure.src.Database;

namespace Server.Infrastructure.src.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;
        public UserRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ChangePasswordAsync(Guid userId, string newPassword, byte[] salt)
        {
            var user = await GetUserByIdAsync(userId);

            if (user == null)
            {
                return false;
            }
            // Update the user's password
            user.Password = newPassword;
            user.Salt = salt;
            // Save the changes to the database
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            return !await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return await GetUserByIdAsync(user.Id);
        }

        public async Task<bool> DeleteUserByIdAsync(Guid id)
        {
            var userToDelete = await GetUserByIdAsync(id);

            // If user with the specified ID exists, delete it
            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
                return true; // Return true indicating successful deletion
            }
            else
            {
                return false; // Return false indicating user not found or deletion failed
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(QueryOptions options)
        {
            IQueryable<User> query = _context.Users;

            // Apply search filter if a search key is provided
            if (!string.IsNullOrWhiteSpace(options.SearchKey))
            {
                query = query.Where(u => u.UserName.Contains(options.SearchKey) ||
                                          u.Email.Contains(options.SearchKey));
            }

            // Apply sorting if sort type and sort order are specified
            if (options.SortBy.HasValue && options.OrderBy.HasValue)
            {
                switch (options.SortBy.Value)
                {
                    case SortType.ByDate:
                        query = options.OrderBy.Value == SortOrder.Ascending ? query.OrderBy(u => u.CreatedAt) : query.OrderByDescending(u => u.CreatedAt);
                        break;
                }
            }

            // Apply pagination
            int skipCount = options.PageSize * (options.PageNo - 1);
            query = query.Skip(skipCount).Take(options.PageSize);

            return await query.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users
                        .Include("Avatar")
                        .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User> UpdateUserByIdAsync(User UserUpdateInfo)
        {
            var userToUpdate = await GetUserByIdAsync(UserUpdateInfo.Id);
            if (userToUpdate == null)
            {
                return null;
            }
            _context.Users.Update(UserUpdateInfo);

            // Save changes to the database
            await _context.SaveChangesAsync();
            return userToUpdate;

        }
        public async Task<User> GetUserByCredentialsAsync(UserCredential userCredential)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(user => user.Email == userCredential.Email && user.Password == userCredential.Password);
            return foundUser;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User> UploadAvatarAsync(Guid userId, byte[] data)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var avatar = await _context.Avatars.FirstOrDefaultAsync(avatar => avatar.UserId == userId);
                if (avatar is not null)
                {
                    avatar.Data = data;
                    _context.Avatars.Update(avatar);
                }
                else
                {
                    avatar = new Avatar { UserId = userId, Data = data };
                    await _context.Avatars.AddAsync(avatar);
                }

                await _context.SaveChangesAsync();
                // update avatarId in user table
                var user = await GetUserByIdAsync(userId);
                user.AvatarId = avatar.Id;
                await UpdateUserByIdAsync(user);
                _context.Users.Update(user);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync(); // commit
                return await GetUserByIdAsync(userId);
            }
            catch (Exception ex)
            {
                // roll back
                await transaction.RollbackAsync();
                return null;
            }
        }
    }
}


