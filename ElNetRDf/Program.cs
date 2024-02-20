using Elnet.Services;
using Elnet.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using src;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient("self", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});
builder.Services.AddHttpClient("rdfox", httpClient =>
{
    var selfUri = new UriBuilder(builder.HostEnvironment.BaseAddress);
    selfUri.Port = 12110;
    httpClient.BaseAddress = selfUri.Uri;
});
builder.Services.AddSingleton<ISparqlQueryHelper, SparqlQueryHelper>();


var app = builder.Build();


await app.RunAsync();
