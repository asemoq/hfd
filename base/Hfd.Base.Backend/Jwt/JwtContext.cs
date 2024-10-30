using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Services;

namespace Hfd.Base.Backend.Jwt;

internal sealed class JwtContext : IBizContext
{
	public JwtContext(IServiceProvider services)
	{
		Services = services;
	}

	/// <inheritdoc />
	public IServiceProvider Services { get; }

	public string ScopeId { get; } = $"{Guid.NewGuid():N}";

	public UserFull UserFull { get; internal set; }

	public CancellationToken Aborted { get; internal set; } = CancellationToken.None;
}