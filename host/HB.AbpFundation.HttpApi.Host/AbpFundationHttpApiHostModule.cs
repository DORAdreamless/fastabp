using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HB.AbpFundation.EntityFrameworkCore;
using HB.AbpFundation.MultiTenancy;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;
using FreeSql;
using HB.AbpFundation.AggregateRoot.RBAC;
using HB.AbpFundation.RBAC;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Volo.Abp.Auditing;
using Volo.Abp.SecurityLog;
using Microsoft.AspNetCore.Hosting;
using HB.AbpFundation.Frameworks;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OpenTelemetry.Resources;
using OpenTelemetry;
using System.Diagnostics;
using OpenTelemetry.Trace;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Threading;
using HB.AbpFundation.Persistences;
using Microsoft.AspNetCore.Http.Extensions;

namespace HB.AbpFundation;

[DependsOn(
    typeof(AbpFundationApplicationModule),
    typeof(AbpFundationEntityFrameworkCoreModule),
    typeof(AbpFundationHttpApiModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpEntityFrameworkCoreMySQLModule),
    //typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    //typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    //typeof(AbpSettingManagementEntityFrameworkCoreModule),
    //typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
    )]
public class AbpFundationHttpApiHostModule : AbpModule
{
    FreeSqlCloud<string> fsql = new FreeSqlCloud<string>();
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL();
        });

        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = MultiTenancyConsts.IsEnabled;
        });
        Configure<AbpAuditingOptions>(options =>
        {
            options.IsEnabled = false;
        });
        Configure<Volo.Abp.AspNetCore.Mvc.AntiForgery.AbpAntiForgeryOptions>(options =>
        {
            options.AutoValidate = false;
        });
        Configure<AbpSecurityLogOptions>(options =>
        {
            options.IsEnabled = false;
        });
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<AbpFundationDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}HB.AbpFundation.Domain.Shared", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpFundationDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}HB.AbpFundation.Domain", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpFundationApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}HB.AbpFundation.Application.Contracts", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpFundationApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}HB.AbpFundation.Application", Path.DirectorySeparatorChar)));
            });
        }

        context.Services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            new Dictionary<string, string>
            {
                {"AbpFundation", "AbpFundation API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "AbpFundation API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "HB.AbpFundation.Domain.Shared.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "HB.AbpFundation.Application.Contracts.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "HB.AbpFundation.HttpApi.xml"));
            });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
            options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
            options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish"));
            options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
            options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi"));
            options.Languages.Add(new LanguageInfo("is", "is", "Icelandic"));
            options.Languages.Add(new LanguageInfo("it", "it", "Italiano"));
            options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
            options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
            options.Languages.Add(new LanguageInfo("ro-RO", "ro-RO", "Română"));
            options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
            options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch"));
            options.Languages.Add(new LanguageInfo("es", "es", "Español"));
            options.Languages.Add(new LanguageInfo("el", "el", "Ελληνικά"));
        });

        //context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddAbpJwtBearer(options =>
        //    {
        //        options.Authority = configuration["AuthServer:Authority"];
        //        options.RequireHttpsMetadata = configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata");
        //        options.Audience = "AbpFundation";
        //    });

        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "AbpFundation:";
        });

        var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("AbpFundation");
       // if (!hostingEnvironment.IsDevelopment())
        {
            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
            dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "AbpFundation-Protection-Keys");
            context.Services.AddSingleton<ConnectionMultiplexer>(redis);
        }

        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]?
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray() ?? Array.Empty<string>()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        fsql.Register("main", () =>
        {
            var db = new FreeSqlBuilder()
            .UseConnectionString(DataType.MySql, configuration.GetConnectionString("Default"))
            .UseAutoSyncStructure(true)
            .UseNoneCommandParameter(true)
            .UseSlave(configuration.GetConnectionString("DefaultReadOnly"))
            .UseMonitorCommand(executing =>
            {
                Log.Logger.Information("准备执行========>");
                Log.Logger.Information(executing.Connection?.ConnectionString);
                Log.Logger.Information(executing.CommandText);
            }, (executed, d) =>
            {
                //Log.Logger.Information("执行完成========>" + d);
                //Log.Logger.Information(executed.Connection?.DataSource);
                //Log.Logger.Information(executed.CommandText);
            })
            .Build();
            db.GlobalFilter.Apply<PersistenceObjectBase>(GlobalFilters.SoftDelete, x => x.IsDeleted != true);
            //db.Aop.CommandAfter += ...
            return db;
        });
        //fsql.EntitySteering = (_, e) =>
        //{
        //    if (e.EntityType == typeof(Tenant))
        //    {
        //        e.DBKey = "main";
        //    }
        //};
        context.Services.AddSingleton<IFreeSql>(fsql);

        //context.Services.AddFreeRepository(typeof(AbpFundationEntityFrameworkCoreModule).Assembly);
        //context.Services.AddScoped<IFreeSql>(r => r.GetService<UnitOfWorkManager>().Orm);
        //context.Services.AddScoped<UnitOfWorkManager>(r => new UnitOfWorkManager(fsql));

       
        context.Services.AddHttpClient();
        context.Services.AddHttpContextAccessor();

        // 配置JWT认证
        context.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtSettings = configuration.GetSection("AuthServer");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
            };
        });

     
        // OpenTelemetry 初始化
        context.Services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder =>
                tracerProviderBuilder
                    .AddSource(DiagnosticsConfig.ActivitySource.Name)
                    .SetResourceBuilder(OpenTelemetry.Resources.ResourceBuilder.CreateDefault()
                        .AddAttributes(new Dictionary<string, object> {
                    {"service.name", DiagnosticsConfig.ServiceName},
                    {"service.version", "1.0.0"},
                    {"deployment.environment", "dev"},
                    {"host.name",DiagnosticsConfig.HostName}
                        }))
                    .AddAspNetCoreInstrumentation(opt =>
                    {
                        opt.RecordException = true;
                        opt.EnrichWithHttpRequest = async (activity, request) =>
                        {
                            activity.SetTag("request.url", request.GetDisplayUrl());
                            activity.SetTag("request.token", request.Headers.GetOrDefault("Authorization"));
                            activity.SetTag("request.method", request.Method);
                            //if (request.HasFormContentType)
                            //{

                            //}
                            //if(request.ContentType == "application/json")
                            //{
                            //    StreamReader sr = new StreamReader(request.Body);
                            //   var requestJson = await sr.ReadToEndAsync();
                            //    activity.SetTag("request.json", requestJson);
                            //}
                        };

                    })
                    .AddHttpClientInstrumentation(opt =>
                    {
                        opt.RecordException = true;
                    })
                    .AddConsoleExporter() // 在控制台输出Trace数据，可选
                    .AddOtlpExporter(opt =>
                    {
                        // 使用HTTP协议上报
                        //opt.Endpoint = new Uri("<http_endpoint>");
                        //opt.Protocol = OtlpExportProtocol.HttpProtobuf;

                        // 使用gRPC协议上报
                                  opt.Endpoint = new Uri("http://tracing-analysis-dc-hz.aliyuncs.com:8090");
                                   opt.Headers = "Authentication=c1kaz10kak@99ac1404210972e_c1kaz10kak@53df7ad2afe8301";
                                   opt.Protocol = OtlpExportProtocol.Grpc; // 使用gRPC协议上报
                    })
             )
            .WithLogging(loggerProviderBuilder=> loggerProviderBuilder.AddConsoleExporter().AddOtlpExporter(opt =>
            {
                // 使用gRPC协议上报
                opt.Endpoint = new Uri("http://tracing-analysis-dc-hz.aliyuncs.com:8090");
                opt.Headers = "Authentication=c1kaz10kak@99ac1404210972e_c1kaz10kak@53df7ad2afe8301";
                opt.Protocol = OtlpExportProtocol.Grpc; // 使用gRPC协议上报
            }));

    }

    public override  void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

       

        app.UseHttpsRedirection();
        app.UseCorrelationId();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }
        app.UseAbpRequestLocalization();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");

            var configuration = context.GetConfiguration();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthScopes("AbpFundation");
        });


        app.Use(async (context, next) =>
        {
            var logger = context.RequestServices.GetService<ILogger<AbpFundationHttpApiHostModule>>();
            try
            {
                var tenantResolver = context.RequestServices.GetService<HB.AbpFundation.RBAC.Query.TenantResolver>();
                // 使用者通过 aspnetcore 中间件，解析 token，查询  main 库得到租户信息。
                // string name = context.Request.Headers.GetOrDefault("tenant");
                string name = context.Request.Query["tid"];

                if (string.IsNullOrWhiteSpace(name))
                {
                    logger.LogInformation("无租户");
                    fsql.Change("main");
                }
                else
                {
                    var tenant = await tenantResolver.GetContextTenantAsync(name);
                    if (tenant == null)
                    {
                        throw new Exception($"缺少租户配置：{name}");
                    }
                    logger.LogInformation("当前租户:" + tenant.Name);
                    // 只会首次注册，如果已经注册过则不生效
                    fsql.Register(tenant.Name, () =>
                    {
                        var builder = new FreeSqlBuilder()
                        .UseConnectionString(DataType.MySql, tenant.MasterConnectionString);
                        if (!string.IsNullOrWhiteSpace(tenant.ReadOnlyConnectionStrings) && tenant.ReadOnlyConnectionStrings.Length > 2)
                        {
                            var connectionStrings = JsonConvert.DeserializeObject<List<string>>(tenant.ReadOnlyConnectionStrings);
                            builder.UseSlave(connectionStrings.ToArray());
                        }
                        var db = builder.UseAutoSyncStructure(true)
                        .UseNoneCommandParameter(true)
                        //.UseMonitorCommand(executing =>
                        //   {
                        //       logger.LogInformation(executing.Connection?.ConnectionString);
                        //       logger.LogInformation(executing.CommandText);
                        //   }, (executed, d) =>
                        //   {
                        //       logger.LogInformation("执行完成========>" + d);
                        //       logger.LogInformation(executed.Connection?.DataSource);
                        //       logger.LogInformation(executed.CommandText);
                        //   })
                        .Build();
                        db.GlobalFilter.Apply<PersistenceObjectBase>(GlobalFilters.SoftDelete, x => x.IsDeleted != true);
                        //db.Aop.AuditValue += Aop_AuditValue;
                        //db.Aop.CommandAfter += ...
                        return db;
                    });

                    // 切换租户
                    fsql.Change(tenant.Name);

                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"按租户分库出现错误:{ex.Message}");
            }
            finally
            {
                await next();
            }
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();

        AsyncHelper.RunSync(async () =>
        {
            await SeedData(context);
        });


    }

    private void Aop_AuditValue(object sender, FreeSql.Aop.AuditValueEventArgs e)
    {
        if (e.Property.Name == "A")
        {

        }
        throw new NotImplementedException();
    }

    private async Task SeedData(ApplicationInitializationContext context)
    {
        using (var scope = context.ServiceProvider.CreateScope())
        {
            await scope.ServiceProvider
                .GetRequiredService<IDataSeeder>()
                .SeedAsync();
        }
    }
}

public static class DiagnosticsConfig
{
    public const string ServiceName = "AbpFundation"; // 服务名
    public const string HostName = "localhost"; // 主机名
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
}

