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
builder.Services.AddHttpClient("BasketAPI", client => client.BaseAddress = new Uri("https://localhost:7021/"));
builder.Services.AddHttpClient("CatalogAPI", client => client.BaseAddress = new Uri("https://localhost:7103/"));
builder.Services.AddHttpClient("ChatAPI", client => client.BaseAddress = new Uri("https://localhost:7229/"));

// Register Frontend Services
builder.Services.AddScoped<blazor_frontend.Services.IAuthService, blazor_frontend.Services.AuthService>();
builder.Services.AddScoped<blazor_frontend.Services.IOrderService, blazor_frontend.Services.OrderService>();
builder.Services.AddScoped<blazor_frontend.Services.IDiscountService, blazor_frontend.Services.DiscountService>();
builder.Services.AddScoped<blazor_frontend.Services.IBasketService, blazor_frontend.Services.BasketService>();
builder.Services.AddScoped<blazor_frontend.Services.ICatalogService, blazor_frontend.Services.CatalogService>();
builder.Services.AddScoped<blazor_frontend.Services.IChatService, blazor_frontend.Services.ChatService>();
builder.Services.AddScoped<blazor_frontend.Services.ICategoryService, blazor_frontend.Services.CategoryService>();
builder.Services.AddScoped<blazor_frontend.Services.IProductService, blazor_frontend.Services.ProductService>();
builder.Services.AddScoped<blazor_frontend.Services.IUserService, blazor_frontend.Services.UserService>();

// NEW: auth state 
builder.Services.AddScoped<blazor_frontend.Services.AuthState>();

await builder.Build().RunAsync();