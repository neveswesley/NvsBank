using MediatR;
using Microsoft.AspNetCore.Identity;
using NvsBank.Application.Exceptions;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Auth.Commands;

public class LoginUser
{
    
    public sealed record LoginUserCommand(string Email, string Password) : IRequest<AuthResponse>;
    
    public sealed record AuthResponse(string acessToken, string refreshToken);
    
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginUserHandler(UserManager<User> userManager, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new NotFoundException("User not found");

            var isValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isValid)
                throw new UnauthorizedException("Incorrect password");
            
            var acessToken = _tokenService.GenerateToken(user);
            
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id);
            
            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            return new AuthResponse(acessToken, refreshToken.Token);
        }
    }
}