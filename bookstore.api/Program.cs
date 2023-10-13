using Microsoft.AspNetCore;

public class Program
{
    private static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        return builder.Build();
    }

    public static void Main(string[] args)
    {
        var configuration = GetConfiguration();
        CreateWebHostBuilder(configuration, args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(IConfiguration configuration, string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .CaptureStartupErrors(true)
            .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseStartup<Startup>()
            .UseUrls(urls: "http://localhost:5200")
        ;
}

