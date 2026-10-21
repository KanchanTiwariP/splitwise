using Microsoft.EntityFrameworkCore;
using SplitWise.Application.Exceptions;
using SplitWise.Application.Interfaces.Repositories;
using SplitWise.Domain.Entities;
using SplitWise.Infrastructure.Persistence;

namespace SplitWise.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
  public readonly AppDbContext _context;
    public RefreshTokenRepository(AppDbContext  dbContext)
    {
        _context = dbContext;
    }
    
    public async Task AddAsync(RefreshToken refreshToken)
    {
       await _context.RefreshToken.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
    {
        var token = await _context.RefreshToken
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);
        return token;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task RotateTokenAsync(
        RefreshToken oldToken,
        RefreshToken newToken)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            oldToken.RevokedAt = DateTime.UtcNow;

            await _context.RefreshToken.AddAsync(newToken);
            await _context.SaveChangesAsync();

            oldToken.ReplacedByTokenId = newToken.Id;
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync();

            throw new UnauthorizedException(
                "Refresh token has already been used.");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}