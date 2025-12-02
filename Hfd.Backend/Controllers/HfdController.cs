using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using Grpc.Core;
using Hfd.BizLayer.Internals;
using Hfd.Common.Backend.Controllers;
using Hfd.Common.Contract.Exceptions;
using Hfd.Common.Contract.Helpers;
using Hfd.Common.Contract.Services;
using Hfd.Common.Models;
using Hfd.Common.Services;
using Hfd.Identity.Exceptions;
using Hfd.Identity.Model;
using Hfd.Provider.Grpc.Model;
using Hfd.Provider.Grpc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestEase;
using Swashbuckle.AspNetCore.Annotations;

namespace Hfd.Backend.Controllers;

/// <summary>
///     Управление сервисом.
/// </summary>
public class HfdController(
    IBizContext context,
    IMapper mapper,
    IProviderService service,
    UserManager<HfdIdentityUser> userManager,
    SignInManager<HfdIdentityUser> signInManager)
    : BaseController(context)
{
    private interface IProviderClient : IAuthorizedClient
    {
        /// <summary>
        /// Изменить язык текущего пользователя.
        /// </summary>
        [Post("/update-language")]
        Task<UserFull> UpdateLanguage(CancellationToken token = default);
    }

    [SwaggerResponse(StatusCodes.Status200OK, "Login information")]
    [HttpPost("/login"), Display(Order = 1)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<bool> Login([FromBody] LoginInfo rq)
    {
        var isValidPhoneNumber = FieldFormatHelper.IsValidPhoneNumber(rq.PhoneNumber);
        if (!isValidPhoneNumber) throw new InvalidPhoneNumberException();
        var phoneNumber = FieldFormatHelper.GetFormattedPhoneNumber(rq.PhoneNumber, false) ?? throw new InvalidPhoneNumberException();
        var user = await userManager.FindByNameAsync(phoneNumber);
        if (user == null) throw new UserNotFoundException();
        var result = await signInManager.PasswordSignInAsync(user, rq.Password, false, false);
        return !result.Succeeded ? throw new UnauthorizedException() : true;
    }

    [SwaggerResponse(StatusCodes.Status200OK, "Login information")]
    [HttpPost("/provider-login"), Display(Order = 2)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<bool> Login([FromQuery] string providerType, [FromBody] object data)
    {
        if (providerType != "tg") return false!;
        if (string.IsNullOrEmpty(data.ToString())) return false!;
        try
        {
            var response = await service.Login(new LoginData() { Data = data.ToString()! });
            var user = await userManager.Users.Where(x => x.PhoneNumber == response.PhoneNumber).FirstOrDefaultAsync();
            if (user == null) throw new UserNotFoundException();
            await signInManager.SignInAsync(user, false);
            return true;
        }
        catch (RpcException e)
        {
            throw new HfdException(e.Message);
        }
    }

    [SwaggerResponse(StatusCodes.Status200OK, "Login information")]
    [HttpPost("/logout"), Display(Order = 1)]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<bool> Logout()
    {
        await signInManager.SignOutAsync();
        return true;
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
        var user = await userManager.FindByIdAsync(this.Context.User?.Id!) ?? throw new UserNotFoundException();
        user.Language = language;
        await userManager.UpdateAsync(user);
        var factory = this.Context.Services.GetRequiredService<IClientFactory<IProviderClient>>();
        var providerOptions = this.Context.Services.GetRequiredService<IProviderOptions>();
        if (providerOptions.TgUri == null) return mapper.Map<UserFull>(user);
        using var client = factory.CreateUserClient(providerOptions.TgUri!);
        await client.UpdateLanguage();
        return mapper.Map<UserFull>(user);
    }
}
