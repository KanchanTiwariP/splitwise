using SplitWise.Domain.Entities;

namespace SplitWise.Application.Interfaces.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}