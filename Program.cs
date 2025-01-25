using TrivialBrick.Services;
using TrivialBrick.UI;
using TrivialBrick.Business;
using TrivialBrick.Data;
using TrivialBrick.Data.Repositories;
using TrivialBrick.Authentication;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); 

// Auth-related services

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

builder.Services.AddScoped<ProtectedSessionStorage>();

// DataLayer-related services
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<ClientRepository>();
builder.Services.AddTransient<AdminRepository>();
builder.Services.AddTransient<AssemblyLineRepository>();
builder.Services.AddTransient<ProductRepository>();
builder.Services.AddTransient<OrderRepository>();
builder.Services.AddTransient<PartRepository>();
builder.Services.AddTransient<ProductPartRepository>();
builder.Services.AddTransient<InstructionRepository>();
builder.Services.AddTransient<InvoiceRepository>();
builder.Services.AddTransient<NotificationRepository>();

// BusinessLayer-related services
builder.Services.AddTransient<BLClients>();
builder.Services.AddTransient<BLAssemblyLines>();
builder.Services.AddTransient<BLCatalog>();
builder.Services.AddTransient<BLOrders>();

builder.Services.AddHostedService<AssemblyLineCheckerService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
