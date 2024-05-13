using AutoMapper;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.AuthServiceAbstract;
using Server.Service.src.Shared;

namespace Server.Service.src.ServiceImplement.AuthServiceImplement;

public class AuthService : IAuthService
{
    private readonly IUserRepo _userRepo;
    private readonly ITokenService _tokenService;
    private readonly IPasswordService _pwdService;
    private readonly IMapper _mapper;

    public AuthService(IUserRepo userRepo, ITokenService tokenService, IMapper mapper, IPasswordService pwdService)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
        _mapper = mapper;
        _pwdService = pwdService;
    }

    public async Task<UserReadDto> GetCurrentProfile(Guid id)
    {
        var foundUser = await _userRepo.GetUserByIdAsync(id);
        if (foundUser != null)
        {
            return _mapper.Map<User, UserReadDto>(foundUser);
        }
        throw CustomException.NotFoundException("User not found");
    }

    public async Task<string> LoginAsync(UserCredential credential)
    {

        var foundByEmail = await _userRepo.GetUserByEmailAsync(credential.Email);
        if (foundByEmail is null)
        {
            throw CustomException.NotFoundException("Email hasn't been registered.");
        }
        var isPasswordMatch = _pwdService.VerifyPassword(credential.Password, foundByEmail.Password, foundByEmail.Salt);
        if (isPasswordMatch)
        {
            return _tokenService.GetToken(foundByEmail);
        }
        throw CustomException.UnauthorizedException("Password incorrect.");
    }
}

