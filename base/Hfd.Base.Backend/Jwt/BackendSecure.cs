using System.Security.Claims;
using AutoMapper;
using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Exceptions;
using Hfd.Base.Contract.Services;
using Microsoft.Extensions.Logging;

namespace Hfd.Base.Backend.Jwt;

internal sealed class BackendSecure : IBackendSecure
{
	private readonly IDbStorage _dataStorage;
	private readonly ILogger<BackendSecure> _log;
	private readonly IMapper _mapper;

	public BackendSecure(IMapper mapper, IDbStorage dataStorage, ILogger<BackendSecure> log)
	{
		(_log, _dataStorage, _mapper) = (log, dataStorage, mapper);
	}

	public async Task<UserFull> Verify(ClaimsPrincipal principal)
	{
		try
		{
			var username = principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
			var iin = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.UserData);

			var user = await _dataStorage.Get<IUserLoginDbService>().GetOne(username);
			if (user.Iin == iin?.Value)
				return _mapper.Map<UserFull>(user);
		}
		catch (Exception ex)
		{
			_log.LogError(ex, "Не удалось проверить принципала.");
		}

		throw new UnauthorizedException();
	}
}

internal interface IBackendSecure
{
	/// <summary>
	///     Exception при неудачной проверке.
	/// </summary>
	/// <param name="principal">Принципал.</param>
	/// <returns></returns>
	Task<UserFull> Verify(ClaimsPrincipal principal);
}