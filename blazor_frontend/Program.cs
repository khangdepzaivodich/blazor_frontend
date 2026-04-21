using blazor_frontend;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add Named HttpClients for Microservices
builder.Services.AddHttpClient("IdentityAPI", client => client.BaseAddress = new Uri("https://localhost:7093/"));
builder.Services.AddHttpClient("OrderingAPI", client => client.BaseAddress = new Uri("https://localhost:7076/"));
builder.Services.AddHttpClient("DiscountAPI", client => client.BaseAddress = new Uri("https://localhost:7002/"));
builder.Services.AddHttpClient("CatalogAPI", client => client.BaseAddress = new Uri("https://localhost:7103/"));

// Register Frontend Services
builder.Services.AddScoped<blazor_frontend.Services.IAuthService, blazor_frontend.Services.AuthService>();
builder.Services.AddScoped<blazor_frontend.Services.IOrderService, blazor_frontend.Services.OrderService>();
builder.Services.AddScoped<blazor_frontend.Services.IDiscountService, blazor_frontend.Services.DiscountService>();
// Admin / Catalog services
builder.Services.AddScoped<blazor_frontend.Services.ICategoryService, blazor_frontend.Services.CategoryService>();
builder.Services.AddScoped<blazor_frontend.Services.IProductService, blazor_frontend.Services.ProductService>();

await builder.Build().RunAsync();