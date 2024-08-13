using CSMS.Entity;
using CSMSBE.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace CSMSBE.Api.PermissionAttribute
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PermissionMiddleware> _logger;

        public PermissionMiddleware(RequestDelegate next, ILogger<PermissionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, ISecurityMatrixService securityMatrixService)
        {   
            if (context.User.Identity.IsAuthenticated)
            {
                var roleIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                if (roleIdClaim != null)
                {
                    context.Items["RoleId"] = roleIdClaim.Value;
                }
                else
                {
                    context.Items["RoleId"] = null;
                }
            }
            await _next(context);
        }
    }
}
