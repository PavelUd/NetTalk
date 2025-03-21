using System.ComponentModel.DataAnnotations;
using Application.Common.Result;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Authentication.Command;

public record RegisterCommand : IRequest<Result<string>>
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
    public string Login { get; set; }
}


internal class  RegisterCommandHandler(IAuthenticationService service,IPasswordEncryptor encryptionService,
        ISymmetricKeyEncryptor keyEncryptor, IUnitOfWork unitOfWork, IUserRepository userRepository)
    : IRequestHandler<RegisterCommand, Result<string>>
{
    private readonly IUserRepository _userRepository = userRepository;

    private readonly List<string> _avatars = new List<string>()
    {
        "https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp",
        "https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava2-bg.webp",
        "https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava3-bg.webp",
        "https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava4-bg.webp",
        "https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava5-bg.webp",
        "https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava6-bg.webp"
    };
    public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var random = new Random();
        try
        {
            if (!IsUniqueLogin(request.Login))
            {
                return await Result<string>.FailureAsync("Пользователь с таким логином уже есть");
            }

            var (passwordHash, salt) = encryptionService.PasswordEncryption(request.Password);
            var (key, iv) = keyEncryptor.GenerateKey();
            var userKey = new SymmetricKey()
            {
                IV = iv,
                Key = key
            };
            var user = new User(request.Email, passwordHash, request.Login, salt,
                _avatars[random.Next(_avatars.Count)], userKey);
            
            await  _userRepository.AddAsync(user);
            await unitOfWork.SaveChangesAsync();
            return await service.Authenticate(request.Email, request.Password);
        }
        catch (Exception e)
        {
            return await Result<string>.FailureAsync(e.Message);
        }
    }

    private bool IsUniqueLogin(string login)
    {
        return ! _userRepository.FindAll().Any(u => u.Login == login);
    }
}
