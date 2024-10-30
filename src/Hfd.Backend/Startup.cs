using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Hfd.Base.Backend;
using Hfd.Base.Backend.Jwt;
using Hfd.Base.Contract.Services;
using Hfd.BizLayer;
using Hfd.Contract;
using Hfd.DbLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Hfd.Backend;

public class Startup
{
	public static readonly Assembly[] Assemblies =
	{
		typeof(Base.Contract.Contract).Assembly,
		typeof(Base.DbLayer.DbLayer).Assembly,
		typeof(Contract.Contract).Assembly,
		typeof(BizLayer.BizLayer).Assembly,
		typeof(DbLayer.DbLayer).Assembly,
		typeof(Backend).Assembly
	};

	/// <inheritdoc cref="IConfiguration" />
	private readonly IConfiguration _configuration;


	/// <inheritdoc cref="IWebHostEnvironment" />
	private readonly IWebHostEnvironment _hostEnvironment;

	public Startup(IWebHostEnvironment environment, IConfiguration configuration)
	{
		_hostEnvironment = environment;
		_configuration = configuration;
	}


	// This method gets called by the runtime. Use this method to add services to the container.
	// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
	public void ConfigureServices(IServiceCollection services)
	{
		services
			.AddHfdContractServices(Assemblies)
			.AddHfdDbLayerServices(_configuration, Assemblies)
			.AddHfdBizLayerServices(Assemblies)
			.AddProviderServices(Assemblies)
			.AddJwtAuthentication()
			.AddScoped<IHfdContext, HfdContext>()
			.AddQuartzJobs(_configuration);

		services
			.AddCors()
			.AddHttpContextAccessor()
			.AddHttpClient();


		services
			.AddControllers()
			.AddMvcOptions(options =>
			{
				options.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
				options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
				options.EnableEndpointRouting = false;
			})
			.AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				options.SerializerSettings.Converters.Add(new StringEnumConverter());
				options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			});

		services
			.AddSwaggerGen(options =>
			{
				options.CustomSchemaIds(t => t.FullName);
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "Hfd. WebApi v1", Version = "v1" });
				options.CustomSchemaIds(s => s.FullName?.Replace("+", "."));
				options.DescribeAllParametersInCamelCase();
				options.EnableAnnotations();

				options.MapType<Stream>(() => new OpenApiSchema { Type = "file" });

				Array.ForEach(Assemblies, a =>
				{
					var xmlFile = $"{a.GetName().Name}.xml";
					var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
					if (File.Exists(xmlPath))
						options.IncludeXmlComments(xmlPath);
				});

				options.TagActionsBy(api =>
				{
					if (api.ActionDescriptor is not ControllerActionDescriptor descriptor)
						return new List<string> { "Другие методы" };
					var attribute = descriptor.ControllerTypeInfo.GetCustomAttribute<DisplayAttribute>();
					return new List<string> { attribute?.Name ?? descriptor.ControllerName };
				});

				options.OrderActionsBy(api =>
				{
					if (api.ActionDescriptor is not ControllerActionDescriptor descriptor) return "9999:zzzz";
					var hi = descriptor.ControllerTypeInfo.GetCustomAttribute<DisplayAttribute>();
					var lo = descriptor.MethodInfo.GetCustomAttribute<DisplayAttribute>();
					var loName = lo?.Order != null ? $"{lo.Order:0000}" : descriptor.ActionName;
					return $"{hi?.Order ?? 9999:0000}:{loName}";
				});

				var securityDefinition = new OpenApiSecurityScheme
				{
					Name = "Bearer",
					BearerFormat = "Jwt",
					Scheme = "bearer",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Description = "Specify the authorization token."
				};

				var securityReference = new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Jwt" }
				};

				var securityRequirement = new OpenApiSecurityRequirement
				{
					{ securityReference, Array.Empty<string>() }
				};

				options.AddSecurityDefinition("Jwt", securityDefinition);
				options.AddSecurityRequirement(securityRequirement);
			})
			.AddSwaggerGenNewtonsoftSupport();
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		app.UseExceptionHandler("/api/error");
		app
			.UseSwagger(o => { o.SerializeAsV2 = false; })
			.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hfd. WebApi v1");
				c.DocExpansion(DocExpansion.None);
				c.RoutePrefix = string.Empty;
			});

		app.UseSerilogRequestLogging();

		app.UseCors(options => options
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader());

		app.UseCookiePolicy(new CookiePolicyOptions
		{
			HttpOnly = HttpOnlyPolicy.Always,
			Secure = CookieSecurePolicy.None,
			MinimumSameSitePolicy = SameSiteMode.Lax
		});

		//app.UseHttpsRedirection();
		app.UseStaticFiles();
		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
	}
}