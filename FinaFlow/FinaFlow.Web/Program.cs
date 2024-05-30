using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FinaFlow.Web;
using MudBlazor.Services;
using FinaFlow.Core;
using FinaFlow.Core.Handlers;
using FinaFlow.Web.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services
    .AddHttpClient(
        WebConfiguration.HttpClientName,
        opt =>
        {
            opt.BaseAddress = new Uri(Configuration.BackendUrl);
        });

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();

await builder.Build().RunAsync();
