using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
namespace Ts.SrcApi.Configurations
{
    public static class ServiceContainer
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
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
                // Done in SwaggerConfigureOptions.cs class
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ts.SrcApi", Version = "v1" });

                // Add swagger security definition
                c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header. \r\n\r\nEnter your username and password in the input field below. \r\n\r\nOutput: \"Basic username:password\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Basic"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Basic"
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
                options.SwaggerDoc(apiDesc.GroupName, new OpenApiInfo { Title = "Ts.SrcApi", Version = apiDesc.ApiVersion.ToString(), Description = "Ts.SrcApi" });
            }
        }
    }
}
