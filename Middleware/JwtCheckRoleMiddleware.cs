using chic_lighting.Services.UserService;

namespace chic_lighting.Middleware
{
    public class JwtCheckRoleMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtCheckRoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserService userService)
        {
            var user = await userService.getCurrentUser();
            if (user.RoleId != 1)
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }

            await _next(context);
        }
    }
}
