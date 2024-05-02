using Project_B_Server.Components;

using Microsoft.AspNetCore.ResponseCompression;
using Project_B_Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Heroku will set the PORT environment variable
builder.WebHost.UseUrls("http://*:" + Environment.GetEnvironmentVariable("PORT"));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Response Compression Middleware services
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// SignalR stuff
app.UseResponseCompression();
app.MapHub<ChatHub>("/chathub");

app.Run();

// TODO Continue from Add a SignalR hub
// https://learn.microsoft.com/en-us/aspnet/core/blazor/tutorials/signalr-blazor?view=aspnetcore-8.0&tabs=netcore-cli
