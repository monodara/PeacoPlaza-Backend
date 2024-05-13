using Server.Core.src.Entity;

namespace Server.Service.src.ServiceAbstract.AuthServiceAbstract;

public interface ITokenService
{
    public string GetToken(User user);

}
