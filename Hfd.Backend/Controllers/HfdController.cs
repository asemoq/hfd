using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using Hfd.Backend.Internals;
using Hfd.BizLayer;
using Hfd.Common.Backend.Jwt;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Models;
using Hfd.Identity.Model;
using Hfd.Provider.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Annotations;
using ValidationException = FluentValidation.ValidationException;

namespace Hfd.Backend.Controllers;

/// <summary>
///     Управление сервисом.
/// </summary>
[SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
[ApiController, Route("hfd")]
[Display(Order = 1, Name = "Управление сервисом")]
public class HfdController(IBackendContext context, UserManager<HfdIdentityUser> userManager, SignInManager<HfdIdentityUser> signInManager)
    : ControllerBase
{
    [SwaggerResponse(StatusCodes.Status200OK, "Login information")]
    [HttpPost("/login"), Display(Order = 6)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<LoginFull> Login([FromBody] LoginInfo rq)
    {
        var user = await userManager.FindByNameAsync(rq.UserName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var result = await signInManager.PasswordSignInAsync(user, rq.Password, false, false);

        if (!result.Succeeded)
        {
            throw new UnauthorizedException();
        }

        var roles = await userManager.GetRolesAsync(user);
        var principal = await context.GetPrincipal(user, roles);
        var handler = context.BizContext.Services.GetRequiredService<IJwtHandler>();
        var token = handler.CreateToken(principal, TimeSpan.FromDays(1));

        var rs = new LoginFull { UserName = principal.Claims.Single(c => c.Type == ClaimTypes.Name).Value, Token = token, };

        return rs;
    }

    [SwaggerResponse(StatusCodes.Status200OK, "Login information")]
    [HttpPost("/provider_login"), Display(Order = 6)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<LoginFull> Login([FromQuery] string providerType, [FromBody] object data)
    {
        var factory = context.BizContext.Services.GetRequiredService<IClientFactory<IProviderClient>>();
        var providerOptions = context.BizContext.Services.GetRequiredService<IProviderOptions>();
        if (providerType != "tg")
        {
            return null!;
        }

        using var client = factory.CreateUserClient(providerOptions.TgUri!);
        var phoneNumber = await client.Login(data);
        var user = await userManager.Users.Where(x => x.PhoneNumber == phoneNumber.ToString()).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var roles = await userManager.GetRolesAsync(user);
        var principal = await context.GetPrincipal(user, roles);
        var handler = context.BizContext.Services.GetRequiredService<IJwtHandler>();
        var token = handler.CreateToken(principal, TimeSpan.FromDays(1));

        var rs = new LoginFull { UserName = principal.Claims.Single(c => c.Type == ClaimTypes.Name).Value, Token = token, };

        return rs;
    }

    [SwaggerResponse(StatusCodes.Status204NoContent, "Password changed")]
    [HttpPut("/password"), Authorize, Display(Order = 7)]
    [Consumes("application/json")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInfo rq, CancellationToken token)
    {
        var user = await userManager.FindByIdAsync(context.BizContext.User.Id) ?? throw new UserNotFoundException();
        var result = await userManager.ChangePasswordAsync(user, rq.OldPassword, rq.NewPassword);
        if (!result.Succeeded)
        {
            throw new HfdException(result.Errors.Select(x => x.Description).ToArray());
        }

        return this.NoContent();
    }

    /// <summary>
    ///     Изменить роль текущего пользователя по умолчанию.
    /// </summary>
    /// <param name="role">Новая роль по умолчанию.</param>
    /// <returns>Учетная запись.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "User has been updated")]
    [HttpPut("/change-default-role")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<HfdIdentityUser> ChangeDefaultRole([FromQuery] UserRole role)
    {
        var user = await userManager.FindByIdAsync(context.BizContext.User.Id) ?? throw new UserNotFoundException();
        var roles = await userManager.GetRolesAsync(user);
        if (roles.Contains(role.ToString()))
        {
            user.DefaultRole = role;
        }

        await userManager.UpdateAsync(user);
        return user;
    }

    /// <summary>
    ///    Изменить язык текущего пользователя.
    /// </summary>
    /// <param name="language">Язык.</param>
    /// <returns>Учетная запись.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "User has been updated")]
    [HttpPut("/change-language")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<HfdIdentityUser> ChangeLanguage([FromQuery] Language language)
    {
        var user = await userManager.FindByIdAsync(context.BizContext.User.Id) ?? throw new UserNotFoundException();
        user.Language = language;
        await userManager.UpdateAsync(user);
        return user;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [ProducesResponseType(typeof(HfdIdentityUser), StatusCodes.Status200OK)]
    [HttpGet("/self"), Authorize]
    [Produces("application/json")]
    [Display(Order = 5)]
    public Task<UserFull> GetUiUserSelf(CancellationToken token = default)
        => Task.FromResult(context.BizContext.User);

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/api/error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error(CancellationToken token)
    {
        var error = this.OverrideErrorDetails() ?? new ErrorDetails();
        var statusCode = StatusCodes.Status418ImATeapot;
        try
        {
            var exception = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;
            statusCode = await this.HandleError(error, exception!, token) ?? StatusCodes.Status400BadRequest;
        }
        catch (Exception ex)
        {
            error.Message = "Ошибка обработки ошибки.";
            error.Details = [ex.Message,];
        }

        var result = new ObjectResult(error) { StatusCode = statusCode };
        result.ContentTypes.Add(MediaTypeNames.Application.Json);
        return result;
    }

    protected virtual ErrorDetails? OverrideErrorDetails()
    {
        return null;
    }

    protected virtual Task<int?> HandleError(ErrorDetails details, Exception exception, CancellationToken token)
    {
        details.Message = "Internal Server Error.";
        HttpStatusCode? statusCode;

        switch (exception)
        {
            case HfdException hfdException:
            {
                details.Message = hfdException.Message;
                details.Details = hfdException.Details.ToArray();
                statusCode = hfdException.GetHttpStatus();
                break;
            }

            case InvalidOperationException invalidOperationException:
            {
                details.Message = invalidOperationException.Message;
                statusCode = HttpStatusCode.MethodNotAllowed;
                break;
            }

            case ValidationException validationException:
            {
                details.Message = "Validation error";
                details.Details = validationException.Errors.Select(s => s.ToString()).ToArray();
                statusCode = HttpStatusCode.BadRequest;
                break;
            }

            default:
            {
                details.Details = [exception.InnerException?.Message ?? exception.InnerException?.StackTrace ?? string.Empty];
                statusCode = HttpStatusCode.InternalServerError;
                break;
            }
        }

        switch (exception.InnerException)
        {
            case HfdException hfdException:
            {
                details.Message = hfdException.Message;
                details.Details = hfdException.Details.ToArray();
                statusCode = hfdException.GetHttpStatus();
                break;
            }
        }

        return Task.FromResult((int?)statusCode);
    }

    /// <summary>
    ///     Обобщенное описание случившейся ошибки.
    /// </summary>
    protected class ErrorDetails
    {
        public string? Message { get; set; }

        public IEnumerable<string>? Details { get; set; }
    }
}
