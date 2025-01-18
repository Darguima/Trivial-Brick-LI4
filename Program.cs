using Microsoft.AspNetCore.Components.Authorization;
using TrivialBrick.Authentication;

using TrivialBrick.Components;
using TrivialBrick.Data;
using TrivialBrick.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Auth-related services
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

// DataLayer-related services
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<ClientRepository>();

// App
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();

