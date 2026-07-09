using Serilog;
using Ts.AdminApp.Configurations;
using Ts.Infra.Ioc.AdminApp;
using WebMarkupMin.AspNetCore8;

try
{
    var builder = WebApplication.CreateBuilder(args);

    //SelfLog.Enable(msg =>
    //File.AppendAllText("serilog-selflog.txt", msg));

    // Add Serilog logger
    Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

    // Add services to the container.
    // Add Serilog
    builder.Host.UseSerilog();
    //builder.Services.AddRazorPages();
    builder.Services.AddServicesForRazorPagesApp(builder.Configuration, builder.Environment);
    builder.Services.RegisterHttpServices(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        // Comment this
        //app.UseExceptionHandler("/exceptionhandler");
        //app.UseStatusCodePagesWithReExecute("/error/{0}");
    }
    else
    {
        app.UseExceptionHandler("/exceptionhandler");
        app.UseStatusCodePagesWithReExecute("/error/{0}");

        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseWebMarkupMin();

    //app.UseSerilogRequestLogging();

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapRazorPages();

    app.Run();
}
catch (Exception ex)
{
    // Serilog: catch setup errors
    Log.Fatal(ex, "Error While building host");
}
finally
{
    Log.CloseAndFlush();
}
