using System.Diagnostics;
using Hfd.Base.Backend.Jwt;
using Hfd.Base.Contract.Dto;
using Hfd.Base.Contract.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hfd.Base.Backend.Quartz;

/// <summary>
///     Декоратор для создания и выполнения задачи.
/// </summary>
[DisallowConcurrentExecution]
[PersistJobDataAfterExecution]
public sealed class QuartzJobRunner : IJob
{
	/// <summary>
	///     Журнал приложения.
	/// </summary>
	private readonly ILogger<QuartzJobRunner> _log;

	/// <summary>
	///     Creates a new IServiceScope that can be used to resolve scoped services.
	/// </summary>
	private readonly IServiceScopeFactory _scopeFactory;

	/// <summary>
	///     Инициализирует новый экземпляр класса <see cref="QuartzJobRunner" />.
	/// </summary>
	/// <param name="scopeFactory">Creates a new IServiceScope.</param>
	/// <param name="log">Журнал приложения.</param>
	public QuartzJobRunner(IServiceScopeFactory scopeFactory, ILogger<QuartzJobRunner> log)
	{
		_scopeFactory = scopeFactory;
		_log = log;
	}

	/// <inheritdoc />
	public async Task Execute(IJobExecutionContext executeContext)
	{
		var quartzContext = new QuartzExecutionContext(executeContext);
		var executorName = quartzContext.ExecutorName;

		_log.LogInformation($"JobRunner<{executorName}> - started at {DateTime.Now:G}");
		var timer = new Stopwatch();
		timer.Start();

		using var scope = _scopeFactory.CreateScope();
		try
		{
			var bizContext = scope.ServiceProvider.GetService<JwtContext>();
			if (bizContext != null) bizContext.UserFull = new UserFull();

			await scope
				.ServiceProvider
				.GetNamedService<IExecutant>(executorName)
				.Execute(quartzContext);
		}
		catch (Exception ex)
		{
			_log.LogError(ex, $"JobRunner<{executorName}> - failure: {ex.Message}.");
			throw new JobExecutionException(ex, false);
		}
		finally
		{
			timer.Stop();
			_log.LogInformation(
				$"JobRunner<{executorName}> - stopped at {DateTime.Now:G}, elapsed = {timer.Elapsed.TotalSeconds:0.000} секунд");
		}
	}
}