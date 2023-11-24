using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using StockPOS.Extensions;
using StockPOS.Settings;
using StockPOS.Models;
using Microsoft.EntityFrameworkCore;
using StockPOS.CustomTokenAuthProvider;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Net.Mime;
using AutoMapper;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost
    .UseKestrel()
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseIISIntegration()
    .ConfigureKestrel((context, options) =>
    {
        options.AllowSynchronousIO = true;
    });

// Add services to the container.

var mysqlDbSettings = builder.Configuration.GetSection(nameof(MysqlDbSettings)).Get<MysqlDbSettings>();
var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));

builder.Services.ConfigureCors(builder.Configuration);
builder.Services.AddDbContext<StockPOSContext>(opt => opt.UseMySql(mysqlDbSettings.ConnectionString, serverVersion));

builder.Services.ConfigureRepositoryWrapper();

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddTransient<TokenProviderMiddleware>();


builder.Services.AddAutoMapper(typeof(Program));


//below 3 lines need to inject IHttpContextAccessor and IActionContextAccessor to AppDb.cs to get login claims
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockPOS", Version = "v1" });
});

builder.Services.AddHealthChecks()
                .AddMySql(
                    mysqlDbSettings.ConnectionString,
                    name: "mysqldb",
                    timeout: TimeSpan.FromSeconds(3),
                    tags: new[] { "ready" }
                );

builder.Services.AddMvc(option => option.EnableEndpointRouting = false)
 .AddNewtonsoftJson(o =>
 {
     o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
     o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
     o.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include;      //it must be Include, otherwise default value (boolean=false, int=0, int?=null, object=null) will be missing in response json			
     o.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
 });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

var app = builder.Build();

app.UseCors("CorsAllowAllPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockPOS v1"));
}
else
{
    //default Error handler for production, need to add response in TokenProviderMiddleware.   
    //All unhandle exceptions will go to there, and actual error log in out log or console
    app.UseExceptionHandler("/Error");
}

// app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

// app.UseAuthorization();

app.UseTokenProviderMiddleware();

// app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
    {
        Predicate = (check) => check.Tags.Contains("ready"),
        ResponseWriter = async (context, report) =>
        {
            var result = JsonSerializer.Serialize(
                new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(entry => new {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                        duration = entry.Value.Duration.ToString()
                    })
                }
            );

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(result);
        }
    });

    endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
    {
        Predicate = (_) => false
    });

    endpoints.MapHealthChecks("/", new HealthCheckOptions
    {
        Predicate = (_) => false
    });
});

app.Run();
