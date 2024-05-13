using Server.Core.src.Entity;
using Server.Core.src.ValueObject;

namespace Server.Service.src.DTO
{
    public class UserReadDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public Guid DefaultAddressId { get; set; }

        public UserReadDto Transform(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Role = user.Role;
            DefaultAddressId = user.DefaultAddressId;
            return this;
        }
    }

    public class UserUpdateDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; private set; }
        public UserUpdateDto(Guid id, string userName = null, string password = null)
        {
            Id = id;
            UserName = userName;
            Password = password;
        }

        public User UpdateUser(User oldUser)
        {
            if (UserName is not null) oldUser.UserName = UserName;
            if (Password is not null) oldUser.Password = Password;
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