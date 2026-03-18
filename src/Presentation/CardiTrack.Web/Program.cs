using CardiTrack.Shared;
using CardiTrack.Web.Components;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// 1. RAZOR COMPONENTS
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. HTTP CLIENT FACTORY — named client targeting the CardiTrack API
builder.Services.AddSingleton<ConfigurationLoader>();
builder.Services.AddHttpClient("CardiTrackApiClient", (sp, client) =>
{
    var loader = sp.GetRequiredService<ConfigurationLoader>();
    client.BaseAddress = new Uri(loader.GetRequired(ConfigurationKeys.Api.BaseUrl));
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();

// MIDDLEWARE PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
