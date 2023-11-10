using Lab_04;
using Microsoft.AspNetCore;

BuildWebHost(args).Build().Run();

static IWebHostBuilder BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>();