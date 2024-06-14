﻿using System.Security.Claims;

namespace core.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal principal, out Guid userId)
    {
        userId = Guid.Empty;
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim is not null && Guid.TryParse(userIdClaim.Value, out userId);
    }
}