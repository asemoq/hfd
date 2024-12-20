using MediatR.Pipeline;

namespace Hfd.Base.BizLayer.Internals;

/// <summary>
///     Validates request before execute.
/// </summary>
/// <typeparam name="TRequest">Request type.</typeparam>
public sealed class ValidationPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
{
	/// <summary>
	///     Requests validator.
	/// </summary>
	private readonly IValidator<TRequest> _validator;

	/// <summary>
	///     Creates an instance of <see cref="ValidationPreProcessor{TRequest}" />.
	/// </summary>
	/// <param name="services">Сервисы приложения.</param>
	public ValidationPreProcessor(IServiceProvider services)
	{
		_validator = services.GetService<IValidator<TRequest>>();
	}

	/// <inheritdoc />
	public async Task Process(TRequest request, CancellationToken token)
	{
		if (_validator != null)
			await _validator.ValidateAndThrowAsync(request, token);
	}
}