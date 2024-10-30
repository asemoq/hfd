using System.Text;
using Hfd.Base.Backend.Jwt;
using Hfd.Base.Backend.Quartz;
using Hfd.Base.Contract.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Quartz.AspNetCore;

namespace Hfd.Base.Backend;

public static class BackendExtensions
{
	#region [ AddQuartzJobs ]

	public static IServiceCollection AddQuartzJobs(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.Configure<QuartzOptions>(configuration.GetSection("Quartz"))
			.AddQuartz(configurator => configurator
				.UseXmlSchedulingConfiguration(x =>
				{
					x.Files = new[] { "~/quartz_jobs.config" };
					x.ScanInterval = TimeSpan.FromSeconds(0);
					x.FailOnFileNotFound = false;
					x.FailOnSchedulingError = true;
				})
				.UseMicrosoftDependencyInjectionJobFactory())
			.AddQuartzServer(options => { options.WaitForJobsToComplete = true; })
			.AddTransient(typeof(QuartzJobRunner));

		return services;
	}

	#endregion

	#region [ AddJwtAuthentication ]

	private const string JwtSecret = "e4f3584de946785d36c2ba566e190f7e2eabc73162611274bec5a912e062b7a6";
	private const string JwtIssuer = "https://hfd.su";

	public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
	{
		var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtSecret));

		services
			.Configure<JwtOptions>(options =>
			{
				options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
				options.Issuer = JwtIssuer;
				options.Audience = JwtIssuer;
			})
			.AddSingleton<IJwtHandler, JwtHandler>()
			.AddScoped<JwtContext, JwtContext>()
			.AddTransient<IUserAccessor>(sp => sp.GetRequiredService<JwtContext>())
			.AddTransient<IBizContext>(sp => sp.GetRequiredService<JwtContext>())
			.AddTransient<IBackendSecure, BackendSecure>()
			.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = false,
					ValidateIssuerSigningKey = true,
					ValidIssuer = JwtIssuer,
					ValidAudience = JwtIssuer,
					IssuerSigningKey = signingKey
				};
				options.Events = new JwtBearerEvents
				{
					OnTokenValidated = async tvc =>
					{
						try
						{
							var context = tvc.HttpContext.RequestServices.GetRequiredService<JwtContext>();

							if (context.UserFull == null)
							{
								var secure = context.Services.GetService<IBackendSecure>();
								context.UserFull = await secure.Verify(tvc.Principal);
							}

							context.Aborted = tvc.HttpContext.RequestAborted;
						}
						catch (Exception ex)
						{
							tvc.Fail(ex);
						}
					}
				};
			});

		return services;
	}

	#endregion
}