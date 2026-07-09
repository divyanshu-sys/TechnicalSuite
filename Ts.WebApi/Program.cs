using Asp.Versioning.ApiExplorer;
using MailKit.Security;
using Microsoft.AspNetCore.Rewrite;
using Rotativa.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Net;
using Ts.Infra.Data;
using Ts.Infra.Ioc.WebApi;
using Ts.WebApi.Configurations;
using Ts.WebApi.Middlewares;
using DotComInfra = Ts.Infra.DotCom.Data;
using DotInInfra = Ts.Infra.DotIn.Data;
using ShopInInfra = Ts.Infra.ShopIn.Data;

try
{
    var builder = WebApplication.CreateBuilder(args);

    //SelfLog.Enable(msg =>
    //File.AppendAllText("serilog-selflog.txt", msg));

    // Add Serilog logger
    if (builder.Environment.IsProduction())
    {
        Log.Logger = new LoggerConfiguration()
                     .WriteTo.Email(
                        options: new()
                        {
                            From = "Web Api Error <error@technicalsuite.com>",
                            To = ["demoemailddj@gmail.com"],
                            Host = "technicalsuite.com",
                            Port = 465,
                            Credentials = new NetworkCredential("error@technicalsuite.com", "d2~cB4u37"),
                            ConnectionSecurity = SecureSocketOptions.SslOnConnect,
                            Subject = new MessageTemplateTextFormatter("Error in Web Api.")
                        },
                        restrictedToMinimumLevel: LogEventLevel.Error,
                        batchingOptions: new()
                        {
                            BatchSizeLimit = 5,
                            BufferingTimeLimit = TimeSpan.FromMinutes(5),
                        })
                    .ReadFrom.Configuration(builder.Configuration)
                    .CreateLogger();
    }
    else
    {
        Log.Logger = new LoggerConfiguration()
         .ReadFrom.Configuration(builder.Configuration)
         .CreateLogger();
    }

    // Add services to the container.
    // Add Serilog
    builder.Host.UseSerilog();
    // Add Health Checks
    builder.Services.AddHealthChecks();

    //builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    //builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen();
    builder.Services.RegisterServices(builder.Configuration);
    builder.Services.AddServicesForApi(builder.Configuration, builder.Environment);
    builder.Services.AddScoped<AutoMigration>();
    builder.Services.AddScoped<DotInInfra.AutoMigration>();
    builder.Services.AddScoped<DotComInfra.AutoMigration>();
    builder.Services.AddScoped<ShopInInfra.AutoMigration>();

    builder.Services.AddDistributedMemoryCache();

    var app = builder.Build();

    var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
    //}

    app.UseMiddleware<CustomExceptionHandlingMiddleware>();

    // Configure Allowed Origins
    var AllowedOrigins = app.Configuration.GetSection("AllowedOrigins:Names").Get<string[]>();
    app.UseCors(x => x
        .WithOrigins(AllowedOrigins).AllowAnyMethod().AllowAnyHeader()
    );

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    //app.UseSerilogRequestLogging();

    app.UseSwagger(c =>
    {
        c.RouteTemplate = "/swagger/{documentName}/swagger.json";
    });
    app.UseSwaggerUI(c =>
    {
        //c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ts.WebAPI v1");
        foreach (var apiDesc in apiVersionProvider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{apiDesc.GroupName}/swagger.json", $"Ts.WebApi {apiDesc.ApiVersion}");
        }

        // Hide Expansion of API and Schemas
        c.DocExpansion(DocExpansion.None);

        if (!app.Environment.IsProduction()) return;
        c.SupportedSubmitMethods();
    });
    app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.MapHealthChecks("/api/healthcheck");

    RotativaConfiguration.Setup(app.Environment.ContentRootPath);

    if (!app.Environment.IsDevelopment())
        using (var scope = app.Services.CreateScope())
        {
            var autoMigration1 = scope.ServiceProvider.GetRequiredService<AutoMigration>();
            autoMigration1.Initialize();

            var autoMigration2 = scope.ServiceProvider.GetRequiredService<DotInInfra.AutoMigration>();
            autoMigration2.Initialize();

            var autoMigration3 = scope.ServiceProvider.GetRequiredService<DotComInfra.AutoMigration>();
            autoMigration3.Initialize();

            var autoMigration4 = scope.ServiceProvider.GetRequiredService<ShopInInfra.AutoMigration>();
            autoMigration4.Initialize();
        }

    app.Run();
}
catch (Exception ex)
{
    // Serilog: catch setup errors
    Log.Fatal(ex, "Error While building host.");
}
finally
{
    Log.CloseAndFlush();
}
