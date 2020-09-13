using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Movie.Demo.API.Helpers;
using Serilog;
using Serilog.Enrichers.AspnetcoreHttpcontext;
using Serilog.Formatting.Compact;
using System.IO;
using System.Linq;
using System.Net;

namespace Movie.Demo.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            CreateWebHostBuilder(args, config).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration config) =>
            WebHost.CreateDefaultBuilder(args)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseConfiguration(config)
            .UseSerilog((provider, context, loggerConfig) =>
            {
                loggerConfig
                .MinimumLevel.ControlledBy(ConfigLogParams.LogLevel)
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Server IP", $"{GetServerIPAddress()}")
                .Enrich.FromLogContext()
                .Enrich.WithAspnetcoreHttpcontext(provider, customMethod: CustomEnricher)
                .WriteTo.Console(new CompactJsonFormatter());
            })
            .UseStartup<Startup>();
        public static string GetServerIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            string ipV4 = string.Empty;
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipV4 = ip.ToString();
                }
            }
            return ipV4;
        }

        private static HttpContextCache CustomEnricher(IHttpContextAccessor context)
        {
            var ctx = context.HttpContext;
            if (context == null) return null;

            var info = new HttpContextCache
            {
                IpAddress = ctx.Connection.RemoteIpAddress.ToString(),
                Host = ctx.Request.Host.ToString(),
                Path = ctx.Request.Path.ToString(),
                IsHttps = ctx.Request.IsHttps,
                Scheme = ctx.Request.Scheme,
                Method = ctx.Request.Method,
                ContentType = ctx.Request.ContentType,
                Protocol = ctx.Request.Protocol,
                QueryString = ctx.Request.QueryString.ToString(),
                Query = ctx.Request.Query.ToDictionary(x => x.Key, y => y.Value.ToString()),
                Headers = ctx.Request.Headers.ToDictionary(x => x.Key, y => y.Value.ToString()),
                Cookies = ctx.Request.Cookies.ToDictionary(x => x.Key, y => y.Value.ToString())
            };

            return info;
        }
    }
}
