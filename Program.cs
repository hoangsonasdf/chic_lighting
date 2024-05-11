using chic_lighting.Middleware;
using chic_lighting.Models;
using chic_lighting.Services.CommonService;
using chic_lighting.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddDbContext<chic_lightingContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("conn");
    options.UseNpgsql(connectionString);
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Key"]))
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.UseWhen(context => context.Request.Path.StartsWithSegments("/User") ||
  context.Request.Path.StartsWithSegments("/Cart") ||
  context.Request.Path.StartsWithSegments("/Order"), builder =>
{
    builder.UseMiddleware<JwtCookieMiddleware>();
});

app.UseWhen(context => context.Request.Path.StartsWithSegments("/User") ||
  context.Request.Path.StartsWithSegments("/Cart") ||
  context.Request.Path.StartsWithSegments("/Dashboard") ||
  context.Request.Path.StartsWithSegments("/Order"), builder =>
{
    builder.UseMiddleware<JwtCookieMiddleware>();
});

app.UseWhen(context => context.Request.Path.StartsWithSegments("/Dashboard"),
    builder =>
{
    builder.UseMiddleware<JwtCheckRoleMiddleware>();
});





app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
