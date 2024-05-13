using Server.Core.src.Common;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.AuthServiceAbstract;

// public interface IAuthService
// {
//     Task<string> LoginAsync(UserCredential userCredential);
// }
public interface IAuthService
{
    public Task<string> Login(UserCredential credential);
    public Task<UserReadDto> GetCurrentProfile(Guid id);
}