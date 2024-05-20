using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.AuthServiceAbstract;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;
using Server.Service.src.Shared;

namespace Server.Service.src.ServiceImplement.EntityServiceImplement
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IPasswordService _pwdService;

        public UserService(IUserRepo userRepo, IPasswordService pwdService)
        {
            _userRepo = userRepo;
            _pwdService = pwdService;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            return await _userRepo.CheckEmailAsync(email);
        }

        // public async Task<UserReadDto> CreateAdminAsync(UserCreateDto user)
        // {
        //     var userToAdd = user.CreateCustomer();
        //     var addedUser = await _userRepo.CreateUserAsync(userToAdd);
        //     return new UserReadDto().Transform(addedUser);
        // 

        public async Task<UserReadDto> CreateCustomerAsync(UserCreateDto userCreateDto)
        {
            var isEmailAvailable = await _userRepo.CheckEmailAsync(userCreateDto.Email);
            if (!isEmailAvailable)
            {
                throw CustomException.BadRequestException("Email has been registered. Maybe try to login...");
            }
            var userToAdd = userCreateDto.CreateCustomer();
            userToAdd.Password = _pwdService.HashPassword(userCreateDto.Password, out byte[] salt);
            userToAdd.Salt = salt;
            var addedUser = await _userRepo.CreateUserAsync(userToAdd);
            return new UserReadDto().Transform(addedUser);
        }

        public async Task<bool> DeleteUserByIdAsync(Guid id)
        {
            var isDeleted = await _userRepo.DeleteUserByIdAsync(id);
            if (!isDeleted)
            {
                throw CustomException.NotFoundException("User is not found.");
            }
            return true;
        }



        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync(QueryOptions options)
        {
            var users = await _userRepo.GetAllUsersAsync(options);
            return users.Select(user => new UserReadDto().Transform(user));
        }

        public async Task<UserReadDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null)
            {
                throw CustomException.NotFoundException("No user found by this id.");
            }
            return new UserReadDto().Transform(user);
        }

        public async Task<UserReadDto> UpdateUserByIdAsync(Guid userId, UserUpdateDto user)
        {
            var userToUpdate = await _userRepo.GetUserByIdAsync(userId);
            if (userToUpdate == null)
            {
                throw CustomException.NotFoundException("No user found to update.");
            }
            var userNewInfo = user.UpdateUser(userToUpdate);
            var updatedUser = await _userRepo.UpdateUserByIdAsync(userNewInfo);
            if (updatedUser == null)
            {
                throw new InvalidOperationException("Updating user failed.");
            }
            return new UserReadDto().Transform(updatedUser);
        }
        public async Task<bool> ChangePasswordAsync(Guid id, string password)
        {
            var userToUpdate = await GetUserByIdAsync(id);
            if (userToUpdate == null)
            {
                throw CustomException.NotFoundException("No user found by this id.");
            }
            //hash password
            var hashedPwd = _pwdService.HashPassword(password, out byte[] salt);
            return await _userRepo.ChangePasswordAsync(id, hashedPwd, salt);
        }

        public async Task<bool> UploadAvatarAsync(Guid userId, byte[] data)
        {
            return await _userRepo.UploadAvatarAsync(userId, data);
        }
    }
}