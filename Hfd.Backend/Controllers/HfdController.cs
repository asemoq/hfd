using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using AutoMapper;
using Hfd.BizLayer.Internals;
using Hfd.Common.Backend.Jwt;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Contract.Helpers;
using Hfd.Common.Contract.Services;
using Hfd.Common.Models;
using Hfd.Common.Services;
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
public class HfdController(
    IBizContext context,
    IMapper mapper,
    UserManager<HfdIdentityUser> userManager,
    SignInManager<HfdIdentityUser> signInManager) : ControllerBase
{
    [SwaggerResponse(StatusCodes.Status200OK, "Login information")]
    [HttpPost("/login"), Display(Order = 1)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<LoginFull> Login([FromBody] LoginInfo rq)
    {
        var isValidPhoneNumber = FieldFormatHelper.IsValidPhoneNumber(rq.PhoneNumber);
        if (!isValidPhoneNumber) throw new HfdException("UserPhoneNumberIsNotValid");
        var phoneNumber = FieldFormatHelper.GetFormattedPhoneNumber(rq.PhoneNumber, false)
                          ?? throw new HfdException("UserPhoneNumberIsNotValid");
        var user = await userManager.FindByNameAsync(phoneNumber);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var result = await signInManager.PasswordSignInAsync(user, rq.Password, false, false);

        if (!result.Succeeded)
        {
            throw new UnauthorizedException();
        }

        var handler = context.Services.GetRequiredService<IJwtHandler>();
        var principal = await handler.GetPrincipal(mapper.Map<UserFull>(user));
        var token = handler.CreateToken(principal, TimeSpan.FromDays(7));

        var rs = new LoginFull { UserName = principal.Claims.Single(c => c.Type == ClaimTypes.Name).Value, Token = token, };

        return rs;
    }

    [SwaggerResponse(StatusCodes.Status200OK, "Login information")]
    [HttpPost("/provider-login"), Display(Order = 2)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<LoginFull> Login([FromQuery] string providerType, [FromBody] object data)
    {
        var factory = context.Services.GetRequiredService<IClientFactory<IProviderClient>>();
        var providerOptions = context.Services.GetRequiredService<IProviderOptions>();
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

        var handler = context.Services.GetRequiredService<IJwtHandler>();
        var principal = await handler.GetPrincipal(mapper.Map<UserFull>(user));

        var token = handler.CreateToken(principal, TimeSpan.FromDays(7));

        var rs = new LoginFull { UserName = principal.Claims.Single(c => c.Type == ClaimTypes.Name).Value, Token = token, };

        return rs;
    }

    [SwaggerResponse(StatusCodes.Status204NoContent, "Password changed")]
    [HttpPut("/password"), Authorize, Display(Order = 3)]
    [Consumes("application/json")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInfo rq, CancellationToken token)
    {
        var user = await userManager.FindByIdAsync(context.User.Id) ?? throw new UserNotFoundException();
        var result = await userManager.ChangePasswordAsync(user, rq.OldPassword, rq.NewPassword);
        return !result.Succeeded ? throw new HfdException(result.Errors.Select(x => x.Description).ToArray()) : this.NoContent();
    }

    /// <summary>
    ///     Изменить роль текущего пользователя по умолчанию.
    /// </summary>
    /// <param name="role">Новая роль по умолчанию.</param>
    /// <returns>Учетная запись.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "User has been updated")]
    [HttpPut("/change-default-role"), Display(Order = 4)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<UserFull> ChangeDefaultRole([FromQuery] UserRole role)
    {
        var user = await userManager.FindByIdAsync(context.User.Id) ?? throw new UserNotFoundException();
        var roles = await userManager.GetRolesAsync(user);
        user.DefaultRole = roles.Contains(role.ToString()) ? role : throw new AccessDeniedException();
        await userManager.UpdateAsync(user);
        return mapper.Map<UserFull>(user);
    }

    /// <summary>
    ///    Изменить язык текущего пользователя.
    /// </summary>
    /// <param name="language">Язык.</param>
    /// <returns>Учетная запись.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "User has been updated")]
    [HttpPut("/change-language"), Authorize, Display(Order = 5)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<UserFull> ChangeLanguage([FromQuery] Language language)
    {
        var user = await userManager.FindByIdAsync(context.User.Id) ?? throw new UserNotFoundException();
        user.Language = language;
        await userManager.UpdateAsync(user);
        var factory = context.Services.GetRequiredService<IClientFactory<IProviderClient>>();
        var providerOptions = context.Services.GetRequiredService<IProviderOptions>();
        if (providerOptions.TgUri == null) return mapper.Map<UserFull>(user);
        using var client = factory.CreateUserClient(providerOptions.TgUri!);
        await client.UpdateLanguage();
        return mapper.Map<UserFull>(user);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [ProducesResponseType(typeof(UserFull), StatusCodes.Status200OK)]
    [HttpGet("/self"), Authorize, Display(Order = 6)]
    [Produces("application/json")]
    public Task<UserFull> GetUiUserSelf(CancellationToken token = default)
        => Task.FromResult(context.User);

    /// <summary>
    ///     Получить учетную запись по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Учетная запись.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [SwaggerResponse(StatusCodes.Status200OK, "User")]
    [HttpGet("/get-user-by-id/{id}"), Authorize(Roles = "Administrator"), Display(Order = 7)]
    [Produces("application/json")]
    public async Task<UserFull> GetUserById([FromRoute] [Required] string id, CancellationToken token)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.Id == id, token);
        return mapper.Map<UserFull>(user) ?? throw new UserNotFoundException();
    }

    /// <summary>
    ///     Изменить роль пользователя по умолчанию.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="role">Новая роль по умолчанию.</param>
    /// <returns>Учетная запись.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "User has been updated")]
    [HttpPut("/change-default-role/{id}"), Authorize(Roles = "Administrator"), Display(Order = 8)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<UserFull> ChangeDefaultRoleById([FromRoute] [Required] string id, [FromQuery] UserRole role)
    {
        var user = await userManager.FindByIdAsync(id) ?? throw new UserNotFoundException();
        var roles = await userManager.GetRolesAsync(user);
        user.DefaultRole = roles.Contains(role.ToString()) ? role : throw new AccessDeniedException();
        await userManager.UpdateAsync(user);
        return mapper.Map<UserFull>(user);
    }

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

            case DataException dataException:
            {
                details.Message = dataException.Message;
                details.Details = exception.GetInnerExceptions().Select(e => e.Message);
                statusCode = HttpStatusCode.InternalServerError;
                break;
            }

            default:
            {
                details.Details = [exception.StackTrace];
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

        public IEnumerable<string?>? Details { get; set; }
    }
}
