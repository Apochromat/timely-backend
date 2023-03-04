using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using timely_backend.Models.DTO;
using timely_backend.Services;

namespace timely_backend;

public class LogoutCheckAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler {
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();
    private readonly ICacheService _cacheService;
    
    public LogoutCheckAuthorizationMiddlewareResultHandler(ICacheService cacheService) {
        _cacheService = cacheService;
    }

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult) {
        
        if (await _cacheService.CheckToken(context.Request.Headers["Authorization"].ToString() ?? "")) {
            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }
        
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}