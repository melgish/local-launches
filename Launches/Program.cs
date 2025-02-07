// spell-checker: words Antiforgery, HSTS, Serilog
using System.Reflection;
using Launches.Components;
using Launches.Services;
using Serilog;

// Use static log during startup to log any configuration warnings or errors.
Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateBootstrapLogger();

try
{
    var name = Assembly.GetExecutingAssembly().GetName();
    Log.Information("{AssemblyName} v{Version}", name.Name, name.Version);

    var builder = WebApplication.CreateBuilder(args);

    // Now that configuration is augmented add the real logger
    // using appsettings and services.
    builder.Services.AddSerilog((services, cfg) => cfg
      .ReadFrom.Configuration(builder.Configuration)
      .ReadFrom.Services(services)
    );

    // Add services to the container.
    builder.Services.AddRazorComponents();

    builder.Services.AddSingleton<IHtmlLoader, HtmlLoader>();
    builder.Services.AddSingleton<ISpaceFlightNow, SpaceFlightNow>();


    builder.Services
        .AddSingleton<ILaunchRepository, LaunchService>()
        .AddOptions<LaunchOptions>()
        .Bind(builder.Configuration.GetSection("Launch"))
        .ValidateDataAnnotations()
        .ValidateOnStart();
    builder.Services.AddHostedService(sp =>
        (LaunchService)sp.GetRequiredService<ILaunchRepository>()
    );

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // app.UseHsts();
    }

    // app.UseHttpsRedirection();

    app.MapStaticAssets();
    app.UseAntiforgery();

    app.MapRazorComponents<App>();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
