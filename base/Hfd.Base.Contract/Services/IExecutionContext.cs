namespace Hfd.Base.Contract.Services;

/// <summary>
///     Контекст исполнения задачи.
/// </summary>
public interface IExecutionContext
{
	CancellationToken Aborted { get; }

	string GetString(string key);

	void PutString(string key, string value);
}