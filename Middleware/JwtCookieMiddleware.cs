namespace chic_lighting.Middleware
{
    public class JwtCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Cookies.TryGetValue("token", out string jwtToken) ||
                string.IsNullOrEmpty(jwtToken))
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }


            await _next(context);
        }
    }
}