using System.IO;
using Hfd.Common.Contract.Attributes;
using Microsoft.Extensions.Logging;

namespace Hfd.BizLayer.Jobs;

/// <summary>
///     Обертка для самопроверочного сообщения HeartbeatMessage.
/// </summary>
[NamedService("Heartbeat:Job")]
public sealed class HeartbeatJob : IExecutant
{
	private static readonly SemaphoreSlim Lock = new(1, 1);
	private readonly ILogger<HeartbeatJob> _log;

	public HeartbeatJob(ILogger<HeartbeatJob> log)
	{
		_log = log;
	}


	/// <inheritdoc />
	public async Task Execute(IExecutionContext context)
	{
		var message = $"{DateTime.Now:G}";

		var file = context.GetString("heartbeatFile");
		if (file != null)
		{
			await Lock.WaitAsync(context.Aborted);
			try
			{
				file = Environment.ExpandEnvironmentVariables(file);
				if (Directory.Exists(Path.GetDirectoryName(file)))
					await File.WriteAllTextAsync(file, message, context.Aborted);
			}
			finally
			{
				Lock.Release();
			}
		}

		_log.LogInformation($"Heartbeat at {message}.");
	}
}