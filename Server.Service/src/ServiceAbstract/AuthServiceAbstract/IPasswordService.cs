namespace Server.Service.src.ServiceAbstract.AuthServiceAbstract;
public interface IPasswordService
{
    public string HashPassword(string password, out byte[] salt);
    public bool VerifyPassword(string password, string passwordHash, byte[] salt);
}
