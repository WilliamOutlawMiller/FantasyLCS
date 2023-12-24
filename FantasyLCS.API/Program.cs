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
            webBuilder.UseKestrel(options =>
            {
                options.Listen(System.Net.IPAddress.Any, 443, listenOptions =>
                {
                    listenOptions.UseHttps("/etc/letsencrypt/live/fantasy-lcs.com/fullchain.pem", "/etc/letsencrypt/live/fantasy-lcs.com/privkey.pem");
                });
            });
        });
}