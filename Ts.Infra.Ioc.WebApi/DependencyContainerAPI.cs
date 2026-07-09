using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Ts.Application.AppConstants;
using Ts.Application.AppInterfaces;
using Ts.Application.AppServices;
using Ts.Application.AppSettings;
using Ts.Common.AppInterfaces;
using Ts.Common.AppServices;
using Ts.Common.Constant.AppConstants;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
using Ts.Infra.Data.Repositories;
using Ts.Infra.DotCom.Data.Context;
using Ts.Infra.DotIn.Data.Context;
using Ts.Infra.ShopIn.Data.Context;
using Ts.Service.ApiSecurity;
using Ts.Service.AutoMapper;
using Ts.ShopIn.Domain.Models;
using DotComAutoMapper = Ts.DotCom.Service.AutoMapper;
using DotComInterfaces = Ts.DotCom.Domain.Interfaces;
using DotComRepositories = Ts.Infra.DotCom.Data.Repositories;
using DotInAutoMapper = Ts.DotIn.Service.AutoMapper;
using DotInInterfaces = Ts.DotIn.Domain.Interfaces;
using DotInRepositories = Ts.Infra.DotIn.Data.Repositories;
using ShopInAutoMapper = Ts.ShopIn.Service.AutoMapper;
using ShopInInterfaces = Ts.ShopIn.Domain.Interfaces;
using ShopInRepositories = Ts.Infra.ShopIn.Data.Repositories;
namespace Ts.Infra.Ioc.WebApi
{
    public static class DependencyContainerApi
    {
        public static void AddServicesForApi(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtSetting:SecretKey"]));
            var signingShopInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtSettingShopIn:SecretKey"]));

            #region Add Database Context and set database to use.
            services.AddDbContextPool<TsIdentityDbContext>(optionBuilder => optionBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("TsIdentityDbConnection"), options =>
                {
                    options.CommandTimeout(60);
                    // Need to comment because giving exception when using transaction
                    //options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
                    options.MigrationsAssembly(typeof(TsIdentityDbContext).Assembly.FullName);
                })
                .ConfigureWarnings(builder =>
                {
                    builder.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
                    builder.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning);
                })
            );

            services.AddDbContextPool<DotInDbContext>(optionBuilder => optionBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("DotInDbConnection"), options =>
                {
                    options.CommandTimeout(60);
                    // Need to comment because giving exception when using transaction
                    //options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
                    options.MigrationsAssembly(typeof(DotInDbContext).Assembly.FullName);
                })
                .ConfigureWarnings(builder =>
                {
                    builder.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
                    builder.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning);
                })
            );

            services.AddDbContextPool<DotComDbContext>(optionBuilder => optionBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("DotComDbConnection"), options =>
                {
                    options.CommandTimeout(60);
                    // Need to comment because giving exception when using transaction
                    //options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
                    options.MigrationsAssembly(typeof(DotComDbContext).Assembly.FullName);
                })
                .ConfigureWarnings(builder =>
                {
                    builder.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
                    builder.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning);
                })
            );

            services.AddDbContextPool<ShopInDbContext>(optionBuilder => optionBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration.GetConnectionString("ShopInDbConnection"), options =>
                {
                    options.CommandTimeout(60);
                    // Need to comment because giving exception when using transaction
                    //options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
                    options.MigrationsAssembly(typeof(ShopInDbContext).Assembly.FullName);
                })
                .ConfigureWarnings(builder =>
                {
                    builder.Ignore(CoreEventId.PossibleIncorrectRequiredNavigationWithQueryFilterInteractionWarning);
                    builder.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning);
                })
            );
            #endregion

            #region Set MaxRequestBodySize
            var AllowedFormSize = Convert.ToInt32(configuration["AllowedFormSize"]);
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = AllowedFormSize;
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = AllowedFormSize; // if not set then default value is: 30 MB
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = AllowedFormSize;
                x.MultipartBodyLengthLimit = AllowedFormSize;
                x.MultipartHeadersLengthLimit = AllowedFormSize;
            });
            #endregion

            #region Lowercasing URL
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
            #endregion

            #region Register Identity service
            services.AddIdentityCore<ApplicationUser>(option =>
            {
                option.SignIn.RequireConfirmedEmail = true;
                option.SignIn.RequireConfirmedAccount = true;
                option.Password.RequiredLength = 8;
                option.Password.RequireDigit = true;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = true;
                option.User.RequireUniqueEmail = true;
                option.Lockout.MaxFailedAccessAttempts = Convert.ToInt32(configuration["MaxLoginFailedAccessAttempts"]);
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(configuration["DefaultLockoutTimeInMinute"]));
            })
                .AddRoles<ApplicationRole>()
                .AddDefaultTokenProviders().AddEntityFrameworkStores<TsIdentityDbContext>();

            services.AddIdentityCore<ClientUser>(option =>
            {
                option.SignIn.RequireConfirmedEmail = true;
                option.SignIn.RequireConfirmedAccount = true;
                option.Password.RequiredLength = 8;
                option.Password.RequireDigit = true;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = true;
                option.User.RequireUniqueEmail = true;
                option.Lockout.MaxFailedAccessAttempts = Convert.ToInt32(configuration["MaxLoginFailedAccessAttempts"]);
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(configuration["DefaultLockoutTimeInMinute"]));
            })
                .AddDefaultTokenProviders().AddEntityFrameworkStores<ShopInDbContext>();

            // Configure Default DataProtecton Token Valid time for all when creating token using Identity service
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(configuration.GetValue<int>("DefaultTokenValidFromMinutes"));
            });
            #endregion

            #region Register Authentication and Authorization
            // Add Authentication
            //services.AddAuthentication();

            //Add JWT Authentication and bearer configuration
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthSchemeConstant.AdminScheme;
                options.DefaultChallengeScheme = AuthSchemeConstant.AdminScheme;
            })
                .AddJwtBearer(AuthSchemeConstant.AdminScheme, configureOptions =>
                    {
                        configureOptions.RequireHttpsMetadata = true;
                        configureOptions.SaveToken = true;
                        configureOptions.ClaimsIssuer = configuration["JwtSetting:Issuer"];
                        configureOptions.TokenValidationParameters = new TokenValidationParameters
                        {
                            ClockSkew = TimeSpan.Zero,
                            IssuerSigningKey = signingKey,
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            RequireExpirationTime = true,
                            ValidateLifetime = true,
                            ValidateAudience = true,
                            ValidAudiences = configuration.GetSection("JwtSetting:Audience").Get<string[]>(),
                            ValidateIssuer = true,
                            ValidIssuer = configuration["JwtSetting:Issuer"]
                        };
                    }).AddJwtBearer(AuthSchemeConstant.PublicScheme, configureOptions =>
                    {
                        configureOptions.RequireHttpsMetadata = true;
                        configureOptions.SaveToken = true;
                        configureOptions.ClaimsIssuer = configuration["JwtSettingShopIn:Issuer"];
                        configureOptions.TokenValidationParameters = new TokenValidationParameters
                        {
                            ClockSkew = TimeSpan.Zero,
                            IssuerSigningKey = signingShopInKey,
                            ValidateIssuerSigningKey = true,
                            RequireSignedTokens = true,
                            RequireExpirationTime = true,
                            ValidateLifetime = true,
                            ValidateAudience = true,
                            ValidAudiences = configuration.GetSection("JwtSettingShopIn:Audience").Get<string[]>(),
                            ValidateIssuer = true,
                            ValidIssuer = configuration["JwtSettingShopIn:Issuer"]
                        };
                    });

            // Add Authorization
            services.AddAuthorization(options =>
            {
                options.InvokeHandlersAfterFailure = false;

                options.AddPolicy(AuthSchemeConstant.AdminScheme,
                    p =>
                    {
                        p.AddAuthenticationSchemes(AuthSchemeConstant.AdminScheme);
                        p.RequireAuthenticatedUser();
                        p.AddRequirements(new ValidateReLoginApiRequirement());
                    });

                options.AddPolicy(AuthSchemeConstant.PublicScheme,
                    p =>
                    {
                        p.AddAuthenticationSchemes(AuthSchemeConstant.PublicScheme);
                        p.RequireAuthenticatedUser();
                    });

                foreach (var privileges in RoleConstant.GetApplicationPrivileges().Values)
                {
                    foreach (var policies in privileges.Values)
                    {
                        foreach (var claim in policies)
                        {
                            options.AddPolicy(claim,
                                p =>
                                {
                                    p.RequireClaim(claim);
                                    p.AddAuthenticationSchemes(AuthSchemeConstant.AdminScheme);
                                });
                        }
                    }
                }
            });
            #endregion

            #region Register Controllers functionality
            services.AddControllers(config =>
            {
                config.Filters.Add(new ProducesAttribute("application/json", "text/json", "application/xml", "text/xml", "text/plain"));

                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            }).AddXmlSerializerFormatters().AddXmlDataContractSerializerFormatters();
            #endregion

            #region Configure Settings from AppSetting
            // Configure Email Setting
            services.Configure<EmailSetting>(configuration.GetSection(nameof(EmailSetting)));
            services.Configure<EmailSettingShopIn>(configuration.GetSection(nameof(EmailSettingShopIn)));

            // Configure Sms Setting
            services.Configure<SmsSetting>(configuration.GetSection(nameof(SmsSetting)));

            // Configure JWT Setting
            services.Configure<JwtSetting>(options =>
            {
                options.Issuer = configuration["JwtSetting:Issuer"];
                options.Audience = configuration.GetSection("JwtSetting:Audience").Get<string[]>();
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512);
                options.TokenExpireTimeInMinute = configuration.GetValue<int>("JwtSetting:TokenExpireTimeInMinute");
                options.RefreshTokenExpireTimeInMinute = configuration.GetValue<int>("JwtSetting:RefreshTokenExpiresInMinute");
            });

            services.Configure<JwtSettingShopIn>(options =>
            {
                options.Issuer = configuration["JwtSettingShopIn:Issuer"];
                options.Audience = configuration.GetSection("JwtSettingShopIn:Audience").Get<string[]>();
                options.SigningCredentials = new SigningCredentials(signingShopInKey, SecurityAlgorithms.HmacSha512);
                options.TokenExpireTimeInMinute = configuration.GetValue<int>("JwtSettingShopIn:TokenExpireTimeInMinute");
                options.RefreshTokenExpireTimeInMinute = configuration.GetValue<int>("JwtSettingShopIn:RefreshTokenExpiresInMinute");
            });
            #endregion

            #region Register AutoMapper
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfileApi(configuration));
                config.AddProfile(new DotInAutoMapper.AutoMapperProfileApi(configuration));
                config.AddProfile(new DotComAutoMapper.AutoMapperProfileApi(configuration));
                config.AddProfile(new ShopInAutoMapper.AutoMapperProfileApi(configuration));
            });
            services.AddSingleton(mapperConfig.CreateMapper());
            #endregion

            #region Register HelperServices
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<ISmsService, SmsService>();
            services.AddSingleton<IRandomService, RandomService>();
            services.AddSingleton<IFileValidationService, FileValidationService>();
            services.AddScoped<IEventService, EventService>();
            #endregion

            #region Register Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DotInInterfaces.IUnitOfWork, DotInRepositories.UnitOfWork>();
            services.AddScoped<DotComInterfaces.IUnitOfWork, DotComRepositories.UnitOfWork>();
            services.AddScoped<ShopInInterfaces.IUnitOfWork, ShopInRepositories.UnitOfWork>();
            #endregion

            #region Register AuthorizationHandler
            // This handler is used to check user to reauthorize
            services.AddScoped<IAuthorizationHandler, ValidateReLoginApiHandler>();
            #endregion

            #region Add CORS
            services.AddCors();
            #endregion
        }
    }
}
