using System.Reflection;
using Hfd.BizLayer;
using Hfd.Common.Backend;
using Hfd.Contract;
using Hfd.DbLayer;
using Hfd.Provider.ApiClient.Services;
using Hfd.Provider.BizLayer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using BackendContext = Hfd.Backend.Internals.BackendContext;
using IBackendContext = Hfd.Backend.Internals.IBackendContext;

namespace Hfd.Backend;

public class Startup
{
    public static readonly Assembly[] Assemblies =
    {
        typeof(Common.Backend.AssemblyInfo).Assembly,
        typeof(Common.BizLayer.AssemblyInfo).Assembly,
        typeof(Common.Contract.AssemblyInfo).Assembly,
        typeof(Common.DbLayer.AssemblyInfo).Assembly,
        typeof(Contract.AssemblyInfo).Assembly,
        typeof(BizLayer.AssemblyInfo).Assembly,
        typeof(DbLayer.AssemblyInfo).Assembly,
        typeof(Backend).Assembly,
    };

    /// <inheritdoc cref="IConfiguration" />
    private readonly IConfiguration _configuration;

    /// <inheritdoc cref="IWebHostEnvironment" />
    private readonly IWebHostEnvironment _hostEnvironment;

    public Startup(IWebHostEnvironment environment, IConfiguration configuration)
    {
        this._hostEnvironment = environment;
        this._configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHfdContractServices(Assemblies)
            .AddHfdDbLayerServices(Assemblies)
            .AddHfdBizLayerServices(this._configuration, Assemblies)
            .AddJwtAuthentication()
            .AddScoped<IBackendContext, BackendContext>()
            .AddQuartzJobs(this._configuration);

        services.AddTransient(typeof(IClientFactory<>), typeof(ClientFactory<>));

        services.AddHttpClient(ClientFactory.UserClientName, (_, _) => { });

        services.AddHttpClient(ClientFactory.ServiceClientName, (_, _) => { });

        services.AddCors().AddHttpContextAccessor().AddHttpClient();

        services.AddControllers()
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

        services.AddSwaggerGen(
                "Hfd. WebApi",
                "v1",
                Assemblies,
                options => options.FixCustomSchemaIds().SetActionTagAndOrder().SetApikeySecurity())
            .AddSwaggerGenNewtonsoftSupport();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseExceptionHandler("/api/error");
        app.UseSwagger();
        app.UseSerilogRequestLogging();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
