using SplitWise.Domain.Entities;

namespace SplitWise.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);

    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);

    Task SaveChangesAsync();
    Task RotateTokenAsync(RefreshToken oldToken, RefreshToken newToken);
}