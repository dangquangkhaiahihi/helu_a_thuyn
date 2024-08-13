using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CSMSBE.Api.PermissionAttribute
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class RequiresPermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _action;
        private readonly string _screen;

        public RequiresPermissionAttribute(string action, string screen)
        {
            _action = action;
            _screen = screen;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<RequiresPermissionAttribute>>();

            var httpContext = context.HttpContext;
            var user = httpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var roleIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (roleIdClaim == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var roleName = roleIdClaim.Value;
            var securityMatrixService = httpContext.RequestServices.GetRequiredService<ISecurityMatrixService>();
            var hasPermission = await securityMatrixService.HasPermissionAsync(roleName, _screen, _action);

            if (!hasPermission)
            {               
                context.Result = new UnauthorizedResult();
            }
        }

    }
}
