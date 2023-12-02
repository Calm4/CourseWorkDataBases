using Lab_05;
using Microsoft.AspNetCore;

BuildWebHost(args).Build().Run();

static IHostBuilder BuildWebHost(string[] args) =>
    Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());