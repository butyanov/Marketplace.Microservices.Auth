using Microsoft.AspNetCore.Identity;

namespace Auth.API.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    /// <summary>
    /// Поле для хранения токенов
    /// </summary>
    public const string RefreshTokensField = nameof(_refreshTokens);
    
    private List<RefreshToken> _refreshTokens = new();
    
    public ApplicationUser()
    {
        Id = Guid.NewGuid();
    }

    public IReadOnlyList<RefreshToken> RefreshTokens
    {
        get => _refreshTokens;
        protected set => _refreshTokens = value as List<RefreshToken> ?? new List<RefreshToken>(value);
    }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        _refreshTokens.Add(refreshToken);
        PurgeExpiredAndEnsureLimit();
    }

    /// <summary>
    /// Удалить refreshToken у пользователя
    /// </summary>
    /// <param name="refreshToken">Токен</param>
    /// <returns>true если успешно</returns>
    public bool RemoveRefreshToken(RefreshToken refreshToken)
    {
        var removed = _refreshTokens.Remove(refreshToken);
        PurgeExpiredAndEnsureLimit();
        return removed;
    }

    public void PurgeExpiredAndEnsureLimit()
        => _refreshTokens = _refreshTokens
            .Where(x => x.IsActive)
            .OrderBy(x => x.Expires)
            .Take(25)
            .ToList();

    public void RemoveAllRefreshTokens() => _refreshTokens.Clear();
}