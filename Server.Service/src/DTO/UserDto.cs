using Server.Core.src.Entity;
using Server.Core.src.ValueObject;

namespace Server.Service.src.DTO
{
    public class UserReadDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public Guid DefaultAddressId { get; set; }
        public Avatar? Avatar { get; set; }
        public string? AvatarBase64 { get; set; }

        public UserReadDto Transform(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Role = user.Role.ToString();
            DefaultAddressId = user.DefaultAddressId;
            Avatar = user.Avatar;
            if (user.Avatar != null && user.Avatar.Data != null)
            {
                AvatarBase64 = Convert.ToBase64String(user.Avatar.Data);
            }
            return this;
        }
    }

    public class UserUpdateDto
    {
        public string UserName { get; set; }
        // public string Password { get; set; }
        public UserUpdateDto(string userName)
        {
            UserName = userName;
            // Password = password;
        }

        public User UpdateUser(User oldUser)
        {
            if (!string.IsNullOrEmpty(UserName)) oldUser.UserName = UserName;
            // if (!string.IsNullOrEmpty(Password)) oldUser.Password = Password;
            return oldUser;
        }
    }

    public class UserCreateDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserCreateDto(string userName, string email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }

        public User CreateCustomer()
        {
            return new User(UserName, Email, Password, Role.Customer);
        }
        // public User CreateAdmin()
        // {
        //     return new User(UserName, Email, Password, Role.Admin);
        // }
    }
}