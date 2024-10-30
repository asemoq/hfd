namespace Hfd.Base.Contract.Services;

/// <summary>
///     Интерфейс исполнителя задачи.
/// </summary>
public interface IExecutant
{
	/// <summary>
	///     Execute the job.
	/// </summary>
	/// <param name="context">Контекст выполнения задачи.</param>
	/// <returns>Асинхронная операция.</returns>
	Task Execute(IExecutionContext context);
}