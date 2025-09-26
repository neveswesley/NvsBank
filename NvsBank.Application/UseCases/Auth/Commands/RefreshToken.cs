using MediatR;
using NvsBank.Application.Exceptions;
using NvsBank.Domain.Interfaces;

namespace NvsBank.Application.UseCases.Auth.Commands;

public class RefreshTokenUser
{
    public sealed record RefreshTokenCommand(string Token) : IRequest<LoginUser.AuthResponse>;

    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, LoginUser.AuthResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public RefreshTokenHandler(
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService,
            IUserRepository userRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<LoginUser.AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.Token, cancellationToken);

            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.IsUsed || refreshToken.IsExpired)
                throw new UnauthorizedException("Invalid refresh token");

            refreshToken.IsUsed = true;
            await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);

            var user = await _userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);
            if (user == null)
                throw new NotFoundException("User not found");

            var newAccessToken = _tokenService.GenerateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user.Id);

            await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

            return new LoginUser.AuthResponse(newAccessToken, newRefreshToken.Token);
        }
    }
}
