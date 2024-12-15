using api.Auth.Model;
using api.Data;
using api.Dtos.Farm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace api.Auth
{
    public static class AuthEndpoints
    {
        public static void AddAuthApi(this WebApplication app)
        {
            // register
            app.MapPost("api/accounts", async (UserManager<SystemUser> userManager, RegisterUserDto dto) =>
            {
                // check user exists
                var user = await userManager.FindByNameAsync(dto.UserName);

                if (user != null)
                    return Results.UnprocessableEntity("Username already taken");

                var newUser = new SystemUser()
                {
                    UserName = dto.UserName,
                    Email = dto.Email,

                };
                
                var createUserResult = await userManager.CreateAsync(newUser, dto.Password);

                if (!createUserResult.Succeeded)
                    return Results.UnprocessableEntity();

                await userManager.AddToRoleAsync(newUser, SystemRoles.Guest);

                return Results.Created();
            });

            //promote to farmer
            app.MapPost("api/addFarmerRole", [Authorize(Roles = SystemRoles.Admin)]  async (UserManager<SystemUser> userManager, AddFarmerRole dto) =>
            {
                // check user exists
                var user = await userManager.FindByNameAsync(dto.UserName);

                if (user == null)
                    return Results.UnprocessableEntity("User does not exist");

                await userManager.AddToRoleAsync(user, SystemRoles.Farmer);

                return Results.Ok();
            });

            //promote to worker
            app.MapPost("api/addWorkerRole", [Authorize(Roles = SystemRoles.Admin)] async (UserManager<SystemUser> userManager, AddWorkerRole dto, ApplicationDBContext context) =>
            {
                // check user exists
                var user = await userManager.FindByNameAsync(dto.UserName);

                if (user == null)
                    return Results.UnprocessableEntity("User does not exist");

                await userManager.AddToRoleAsync(user, SystemRoles.Worker);

                var farm = await context.Farms.FirstOrDefaultAsync(x => x.Id == dto.FarmId);

                if (farm == null)
                {
                    return Results.UnprocessableEntity("Farm does not exist");
                }

                farm.UserId.Add(user.Id);

                await context.SaveChangesAsync();

                return Results.Ok();
            });


            //login
            app.MapPost("api/login", async (UserManager<SystemUser> userManager, SessionService sessionService, JwtTokenService jwtTokenService, HttpContext httpContext, LoginDto dto) =>
            {
                // check user exists
                var user = await userManager.FindByNameAsync(dto.UserName);

                if (user == null)
                    return Results.UnprocessableEntity("User does not exist");

                var isPasswordValid = await userManager.CheckPasswordAsync(user, dto.Password);

                if(!isPasswordValid)
                    return Results.UnprocessableEntity("Username or password was incorrect");

                var roles = await userManager.GetRolesAsync(user);

                var sessionId = Guid.NewGuid();
                var expiresAt = DateTime.UtcNow.AddDays(3);
                var accessToken = jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
                var refreshToken = jwtTokenService.CreateRefreshToken(sessionId, user.Id, expiresAt);

                await sessionService.CreateSessionAsync(sessionId, user.Id, refreshToken, expiresAt);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = expiresAt
                    //Secure = false
                };

                httpContext.Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);

                return Results.Ok(new SuccessfulLoginDto(accessToken));
            });

            app.MapPost("api/accessToken", async (UserManager<SystemUser> userManager, SessionService sessionService, JwtTokenService jwtTokenService, HttpContext httpContext) =>
            {
                if (!httpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
                    return Results.UnprocessableEntity();

                if(!jwtTokenService.TryParseRefreshToken(refreshToken, out var claims))
                    return Results.UnprocessableEntity();

                var sessionId = claims.FindFirstValue("SessionId");
                if (string.IsNullOrWhiteSpace(sessionId))
                    return Results.UnprocessableEntity();

                var sessionIdAsGuid = Guid.Parse(sessionId);
                if (!await sessionService.IsSessionValidAsync(sessionIdAsGuid, refreshToken))
                    return Results.UnprocessableEntity();

                var userId = claims.FindFirstValue(JwtRegisteredClaimNames.Sub);
                var user = await userManager.FindByIdAsync(userId);
                if(user == null)
                    return Results.UnprocessableEntity();

                var roles = await userManager.GetRolesAsync(user);

                var expiresAt = DateTime.UtcNow.AddDays(3);
                var accessToken = jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
                var newRefreshToken = jwtTokenService.CreateRefreshToken(sessionIdAsGuid, user.Id, expiresAt);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = expiresAt
                    //Secure = false
                };

                httpContext.Response.Cookies.Append("RefreshToken", newRefreshToken, cookieOptions);

                await sessionService.ExtendSessionAsync(sessionIdAsGuid, newRefreshToken, expiresAt);

                return Results.Ok(new SuccessfulLoginDto(accessToken));
            });

            app.MapPost("api/logout", async (UserManager<SystemUser> userManager, SessionService sessionService, JwtTokenService jwtTokenService, HttpContext httpContext) =>
            {
                if (!httpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken))
                    return Results.UnprocessableEntity();

                if (!jwtTokenService.TryParseRefreshToken(refreshToken, out var claims))
                    return Results.UnprocessableEntity();

                var sessionId = claims.FindFirstValue("SessionId");
                if (string.IsNullOrWhiteSpace(sessionId))
                    return Results.UnprocessableEntity();

                var sessionIdAsGuid = Guid.Parse(sessionId);
                if (!await sessionService.IsSessionValidAsync(sessionIdAsGuid, refreshToken))
                    return Results.UnprocessableEntity();

                await sessionService.InvalidateSessionAsync(sessionIdAsGuid);
                httpContext.Response.Cookies.Delete("RefreshToken");

                return Results.Ok();
            });
        }
    }

    public record RegisterUserDto(string UserName, string Email, string Password);
    public record AddFarmerRole(string UserName);
    public record AddWorkerRole(string UserName, int FarmId);
    public record LoginDto(string UserName, string Password);
    public record SuccessfulLoginDto(string AccessToken);
}
