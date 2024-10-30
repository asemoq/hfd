using Hfd.Base.Contract.Services;
using Quartz;

namespace Hfd.Base.Backend.Quartz;

/// <inheritdoc />
internal sealed class QuartzExecutionContext : IExecutionContext
{
	/// <summary>
	///     Контекст <see cref="IJobExecutionContext" />.
	/// </summary>
	private readonly IJobExecutionContext _quartzContext;

	/// <summary>
	///     Создает контекст обработчика.
	/// </summary>
	/// <param name="quartzContext">Контекст <see cref="IJobExecutionContext" />.</param>
	/// <returns>Контекст обработчика.</returns>
	public QuartzExecutionContext(IJobExecutionContext quartzContext)
	{
		_quartzContext = quartzContext;
	}

	/// <summary>
	///     Название исполнителя задачи.
	/// </summary>
	public string ExecutorName
		=> _quartzContext.JobDetail.Key.Name;

	/// <summary>
	///     Токен отмены
	/// </summary>
	public CancellationToken Aborted
		=> _quartzContext.CancellationToken;

	/// <summary>
	///     Retrieve the identified value from <see cref="JobDataMap" />.
	/// </summary>
	/// <param name="key">Identifier of value.</param>
	/// <returns>Identified value.</returns>
	public string GetString(string key)
	{
		return _quartzContext.MergedJobDataMap.GetString(key);
	}

	public void PutString(string key, string value)
	{
		_quartzContext.MergedJobDataMap.Put(key, value);
	}
}