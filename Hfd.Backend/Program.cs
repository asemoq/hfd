using System.Reflection;
using Hfd.Backend.Internals;
using Hfd.BizLayer;
using Hfd.BizLayer.Internals;
using Hfd.Common.Backend;
using Hfd.Common.Services;
using Hfd.Contract;
using Hfd.DbLayer;
using Hfd.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var corsPolicyName = "HFDCorsPolicy";
Assembly[] assemblies =
[
    typeof(Hfd.Common.Backend.AssemblyInfo).Assembly,
    typeof(Hfd.Common.BizLayer.AssemblyInfo).Assembly,
    typeof(Hfd.Common.Contract.AssemblyInfo).Assembly,
    typeof(Hfd.Common.DbLayer.AssemblyInfo).Assembly,
    typeof(Hfd.Identity.AssemblyInfo).Assembly,
    typeof(Hfd.Contract.AssemblyInfo).Assembly,
    typeof(Hfd.BizLayer.AssemblyInfo).Assembly,
    typeof(Hfd.DbLayer.AssemblyInfo).Assembly,
    typeof(Hfd.Backend.AssemblyInfo).Assembly,
];

System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("en-US");
System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en-US");
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo("en-US");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHfdContractServices(assemblies)
    .AddHfdDbLayerServices(assemblies)
    .AddHfdBizLayerServices(builder.Configuration, assemblies)
    .AddIdentityServices(builder.Configuration)
    .AddQuartzJobs(builder.Configuration)
    .AddTransient<IAdminClientFactory, AdminClientFactory>();
builder.Services.AddServices();
builder.Services.AddHttpContextAccessor().AddHttpClient();
builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
    .AddMvcOptions(options =>
    {
        options.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
        options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    });

builder.Services.AddSwaggerGen("HFD API", "v1", assemblies, options => options.FixCustomSchemaIds().SetActionTagAndOrder().SetJwtSecurity())
    .AddSwaggerGenNewtonsoftSupport()
    .AddCors(corsPolicyName);

var app = builder.Build();
app.UseExceptionHandler("/api/error");
app.UseStaticFiles();
app.UseSwagger();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors(corsPolicyName);
app.Services.MigrateHfdDbLayer();

await IdentitySeedData.EnsurePopulated(app.Services);
await app.RunAsync();
