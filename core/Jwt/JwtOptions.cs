namespace core.Jwt;

public record JwtOptions(long AccessTokenLifetimeSeconds, long RefreshTokenLifetimeSeconds, string Secret);