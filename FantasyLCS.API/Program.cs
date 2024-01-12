using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
            {
                // Use Kestrel only in the Development environment for local debugging.
                webBuilder.UseKestrel(options =>
                {
                    options.Listen(System.Net.IPAddress.Loopback, 5000); // Listen on localhost and port 5000
                });
            }
        });
}