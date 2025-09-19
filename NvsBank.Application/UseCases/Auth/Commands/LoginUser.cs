using MediatR;
using Microsoft.AspNetCore.Identity;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Auth.Commands;

public class LoginUser
{
    public sealed record LoginUserCommand(string Email, string Password) : IRequest<string>;
    
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public LoginUserHandler(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new ApplicationException("User not found");

            var isValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isValid)
                throw new ApplicationException("Incorrect password");

            return _tokenService.GenerateToken(user);
        }
    }
}