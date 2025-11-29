using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using AutoMapper;
using Hfd.BizLayer.Internals;
using Hfd.Common.Backend.Jwt;
using Hfd.Common.Contract;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Contract.Helpers;
using Hfd.Common.Contract.Services;
using Hfd.Common.Models;
using Hfd.Common.Services;
using Hfd.Identity.Controllers;
using Hfd.Identity.Exceptions;
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
    SignInManager<HfdIdentityUser> signInManager)
    : IdentityController(context, mapper, userManager)
{
    [SwaggerResponse(StatusCodes.Status200OK, "Login information")]
    [HttpPost("/login"), Display(Order = 1)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<LoginFull> Login([FromBody] LoginInfo rq)
    {
        var isValidPhoneNumber = FieldFormatHelper.IsValidPhoneNumber(rq.PhoneNumber);
        if (!isValidPhoneNumber) throw new InvalidPhoneNumberException();
        var phoneNumber = FieldFormatHelper.GetFormattedPhoneNumber(rq.PhoneNumber, false) ?? throw new InvalidPhoneNumberException();
        var user = await this.UserManager.FindByNameAsync(phoneNumber);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var result = await signInManager.PasswordSignInAsync(user, rq.Password, false, false);

        if (!result.Succeeded)
        {
            throw new UnauthorizedException();
        }

        var handler = this.Context.Services.GetRequiredService<IJwtHandler>();
        var principal = await handler.GetPrincipal(this.Mapper.Map<UserFull>(user));
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
        var factory = this.Context.Services.GetRequiredService<IClientFactory<IProviderClient>>();
        var providerOptions = this.Context.Services.GetRequiredService<IProviderOptions>();
        if (providerType != "tg")
        {
            return null!;
        }

        using var client = factory.CreateUserClient(providerOptions.TgUri!);
        var phoneNumber = await client.Login(data);
        var user = await this.UserManager.Users.Where(x => x.PhoneNumber == phoneNumber.ToString()).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var handler = this.Context.Services.GetRequiredService<IJwtHandler>();
        var principal = await handler.GetPrincipal(this.Mapper.Map<UserFull>(user));

        var token = handler.CreateToken(principal, TimeSpan.FromDays(7));

        var rs = new LoginFull { UserName = principal.Claims.Single(c => c.Type == ClaimTypes.Name).Value, Token = token, };

        return rs;
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
        var user = await this.UserManager.FindByIdAsync(this.Context.User.Id) ?? throw new UserNotFoundException();
        user.Language = language;
        await this.UserManager.UpdateAsync(user);
        var factory = this.Context.Services.GetRequiredService<IClientFactory<IProviderClient>>();
        var providerOptions = this.Context.Services.GetRequiredService<IProviderOptions>();
        if (providerOptions.TgUri == null) return this.Mapper.Map<UserFull>(user);
        using var client = factory.CreateUserClient(providerOptions.TgUri!);
        await client.UpdateLanguage();
        return this.Mapper.Map<UserFull>(user);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [ProducesResponseType(typeof(UserFull), StatusCodes.Status200OK)]
    [HttpGet("/self"), Authorize, Display(Order = 6)]
    [Produces("application/json")]
    public Task<UserFull> GetUiUserSelf(CancellationToken token = default)
        => Task.FromResult(this.Context.User);

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
                details.Message = "validation_error";
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
