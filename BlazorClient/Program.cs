
using BlazorClient;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://snakescores.azurewebsites.net/highscore/") });
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5125/highscore/") });

var app = builder.Build();
await app.RunAsync();
