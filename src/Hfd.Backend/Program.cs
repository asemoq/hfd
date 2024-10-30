using System.Text;
using Hfd.DbLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Hfd.Backend;

public class Program
{
	public static async Task Main(string[] args)
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		Console.OutputEncoding = Encoding.UTF8;
		Console.Title = @"[ Hfd ] - Notification management system";

		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
			.Enrich.FromLogContext()
			.WriteTo.Console()
			.CreateBootstrapLogger();

		try
		{
			var host = Host
				.CreateDefaultBuilder(args)
				.UseSerilog((context, services, configuration) => configuration.ReadFrom
					.Configuration(context.Configuration)
					.ReadFrom.Services(services)
					.Enrich.FromLogContext()
					.WriteTo.Console())
				.ConfigureWebHostDefaults(webBuilder => webBuilder
					.UseStartup<Startup>())
				.Build();

			var scopeFactory = host.Services
				.GetRequiredService<IServiceScopeFactory>();
			using (var scope = scopeFactory.CreateScope())
			{
				scope.ServiceProvider.MigrateHfdDbLayer();
				scope.ServiceProvider.FillDbByDummyData();
			}

			host.Services.GetService<ILoggerFactory>().CreateLogger<Startup>().LogWarning("Hfd стартовал");
			await host.RunAsync();
		}
		catch (Exception ex)
		{
			Log.Error(ex.Message);
		}
		finally
		{
			Log.CloseAndFlush();
		}
	}
}