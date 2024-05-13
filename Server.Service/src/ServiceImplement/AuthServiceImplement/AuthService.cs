using AutoMapper;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.AuthServiceAbstract;
using Server.Service.src.Shared;

namespace Server.Service.src.ServiceImplement.AuthServiceImplement;

// public class AuthService : IAuthService
// {
//     private readonly IUserRepo _userRepo;
//     private readonly ITokenService _tokenService;
//     public AuthService(IUserRepo userRepo, ITokenService tokenService)
//     {
//         _userRepo = userRepo;
//         _tokenService = tokenService;
//     }
//     public async Task<string> LoginAsync(UserCredential userCedential)
//     {
//         User foundUser = await _userRepo.GetUserByCredentialsAsync(userCedential);
//         Console.WriteLine("Auth Service: " + foundUser);

//         if (foundUser == null)
//         {
//             throw new UnauthorizedAccessException("Invalid Credentials");
//         }

//         Console.WriteLine("Auth Service Before calling token: ");


//         return _tokenService.GetToken(foundUser);
//     }
// }

public class AuthService : IAuthService
{
    private IUserRepo _userRepo;
    private ITokenService _tokenService;
    private IMapper _mapper;

    public AuthService(IUserRepo userRepo, ITokenService tokenService, IMapper mapper)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
        _mapper = mapper;
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

    public async Task<string> Login(UserCredential credential)
    {
        var foundByEmail = await _userRepo.GetUserByCredentialsAsync(credential);
        if (foundByEmail is null)
        {
            throw CustomException.NotFoundException("Email not found");
        }
        var isPasswordMatch = PasswordService.VerifyPassword(credential.Password, foundByEmail.Password, foundByEmail.Salt);
        if (isPasswordMatch)
        {
            return _tokenService.GetToken(foundByEmail);
        }
        throw CustomException.UnauthorizedException("Password incorrect");
    }
}

