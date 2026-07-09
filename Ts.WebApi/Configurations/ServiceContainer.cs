using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Ts.Application.AppInterfaces;
using Ts.Application.AppServices;
using Ts.Service.DataInterfaces;
using Ts.Service.DataServices;
using DotComClientInterfaces = Ts.DotCom.Service.HttpClientServices.ClientInterfaces;
using DotComClientServices = Ts.DotCom.Service.HttpClientServices.ClientServices;
using DotComDataInterfaces = Ts.DotCom.Service.DataInterfaces;
using DotComDataServices = Ts.DotCom.Service.DataServices;
using DotComHostedServices = Ts.DotCom.Service.HostedServices;
using DotComHttpClientServices = Ts.DotCom.Service.HttpClientServices;
using DotInClientInterfaces = Ts.DotIn.Service.HttpClientServices.ClientInterfaces;
using DotInClientServices = Ts.DotIn.Service.HttpClientServices.ClientServices;
using DotInDataInterfaces = Ts.DotIn.Service.DataInterfaces;
using DotInDataServices = Ts.DotIn.Service.DataServices;
using DotInHostedServices = Ts.DotIn.Service.HostedServices;
using DotInHttpClientServices = Ts.DotIn.Service.HttpClientServices;
using ShopInClientInterfaces = Ts.ShopIn.Service.HttpClientServices.ClientInterfaces;
using ShopInClientServices = Ts.ShopIn.Service.HttpClientServices.ClientServices;
using ShopInDataInterfaces = Ts.ShopIn.Service.DataInterfaces;
using ShopInDataServices = Ts.ShopIn.Service.DataServices;
using ShopInHostedServices = Ts.ShopIn.Service.HostedServices;
using ShopInHttpClientServices = Ts.ShopIn.Service.HttpClientServices;
namespace Ts.WebApi.Configurations
{
    public static class ServiceContainer
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Register Data Services
            services.AddSingleton<DotInDataInterfaces.IPostViewCounter, DotInDataServices.PostViewCounter>();
            services.AddHostedService<DotInHostedServices.IncrementPostView2HostedService>();
            services.AddSingleton<DotComDataInterfaces.IPostViewCounter, DotComDataServices.PostViewCounter>();
            services.AddHostedService<DotComHostedServices.IncrementPostView2HostedService>();

            services.AddSingleton<DotInDataInterfaces.IStoryViewCounter, DotInDataServices.StoryViewCounter>();
            services.AddHostedService<DotInHostedServices.IncrementStoryView2HostedService>();
            services.AddSingleton<DotComDataInterfaces.IStoryViewCounter, DotComDataServices.StoryViewCounter>();
            services.AddHostedService<DotComHostedServices.IncrementStoryView2HostedService>();
            services.AddSingleton<ShopInDataInterfaces.IBlogViewCounter, ShopInDataServices.BlogViewCounter>();
            services.AddHostedService<ShopInHostedServices.IncrementBlogAndProductDetailView2HostedService>();
            services.AddSingleton<ShopInDataInterfaces.IProductDetailViewCounter, ShopInDataServices.ProductDetailViewCounter>();

            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<IPostOfficeService, PostOfficeService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<IShopCategoryService, ShopCategoryService>();
            services.AddScoped<IDropDownService, DropDownService>();
            services.AddScoped<DotInDataInterfaces.IPostService, DotInDataServices.PostService>();
            services.AddScoped<DotComDataInterfaces.IPostService, DotComDataServices.PostService>();
            services.AddScoped<DotInDataInterfaces.IStoryService, DotInDataServices.StoryService>();
            services.AddScoped<DotComDataInterfaces.IStoryService, DotComDataServices.StoryService>();
            services.AddScoped<ShopInDataInterfaces.IBlogService, ShopInDataServices.BlogService>();
            services.AddScoped<ShopInDataInterfaces.IClientUserService, ShopInDataServices.ClientUserService>();
            services.AddScoped<ShopInDataInterfaces.IProductService, ShopInDataServices.ProductService>();
            services.AddScoped<ShopInDataInterfaces.IProductDetailService, ShopInDataServices.ProductDetailService>();
            services.AddScoped<ShopInDataInterfaces.IHomeService, ShopInDataServices.HomeService>();
            services.AddScoped<ShopInDataInterfaces.ICartService, ShopInDataServices.CartService>();
            services.AddScoped<ShopInDataInterfaces.IRazorpayService, ShopInDataServices.RazorpayService>();
            services.AddScoped<ShopInDataInterfaces.IOrderService, ShopInDataServices.OrderService>();
            #endregion

            #region Register Helper services
            services.AddSingleton<IEmailMessageService, EmailMessageService>();
            services.AddSingleton<ShopInDataInterfaces.IEmailShopInMessageService, ShopInDataServices.EmailShopInMessageService>();
            services.AddSingleton<ISmsMessageService, SmsMessageService>();
            #endregion

            #region Register Http client
            services.AddHttpClient();

            services.AddScoped<DotInHttpClientServices.IHttpFileClientService, DotInHttpClientServices.HttpFileClientService>();
            services.AddScoped<DotInClientInterfaces.IFileClient, DotInClientServices.FileClient>();

            services.AddScoped<DotComHttpClientServices.IHttpFileClientService, DotComHttpClientServices.HttpFileClientService>();
            services.AddScoped<DotComClientInterfaces.IFileClient, DotComClientServices.FileClient>();

            services.AddScoped<ShopInHttpClientServices.IHttpFileClientService, ShopInHttpClientServices.HttpFileClientService>();
            services.AddScoped<ShopInClientInterfaces.IFileClient, ShopInClientServices.FileClient>();
            #endregion

            #region Register API Versioning
            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
            })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
            #endregion

            #region Add SwaggerGen
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                // Use full type name as schema ID
                c.CustomSchemaIds(type => type.FullName);

                // Done in SwaggerConfigureOptions.cs class
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ts.Webapi", Version = "v1" });

                // Add swagger security definition
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\nEnter Bearer [space] and then your token in the input field below. \r\n\r\nExample: \"Bearer abcdefgh\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                // Include Xml comments in documentation.
                if (XmlCommentsFilePath != null)
                    c.IncludeXmlComments(XmlCommentsFilePath);
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
            #endregion
        }

        #region Get Xml comments to display on swagger
        private static string XmlCommentsFilePath
        {
            get
            {
                var fileName = typeof(ServiceContainer).GetTypeInfo().Assembly.GetName().Name + ".xml";
                string filePath = Path.Combine(AppContext.BaseDirectory, fileName);
                if (File.Exists(filePath))
                    return filePath;
                return null;
            }
        }
        #endregion
    }

    public class SwaggerConfigureOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionProvider;

        public SwaggerConfigureOptions(IApiVersionDescriptionProvider apiVersionProvider) => _apiVersionProvider = apiVersionProvider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var apiDesc in _apiVersionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(apiDesc.GroupName, new OpenApiInfo { Title = "Ts.WebApi", Version = apiDesc.ApiVersion.ToString(), Description = "Ts.WebApi" });
            }
        }
    }
}
