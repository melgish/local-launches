using Launches.Components;
using Launches.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    // .AddInteractiveServerComponents()
    ;

builder.Services.AddSingleton<ILaunchRepository, LaunchService>();
builder.Services.AddHostedService(sp =>
    (LaunchService)sp.GetRequiredService<ILaunchRepository>()
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    // .AddInteractiveServerRenderMode()
    ;

app.Run();
