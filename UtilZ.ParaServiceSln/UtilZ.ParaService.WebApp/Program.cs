using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UtilZ.ParaService.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("hosting.json", optional: true)
            .Build();
            CreateWebHostBuilder(args, config).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration config) =>
            WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())//定义contentroot  
                .UseWebRoot(Directory.GetCurrentDirectory() + "/wwwroot")//定义webroot
                //.UseUrls("http://*:12018", "https://*:22018")
                .UseStartup<Startup>();
    }
}
