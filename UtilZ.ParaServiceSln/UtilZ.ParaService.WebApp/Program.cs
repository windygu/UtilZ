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
            //Models.WebAppConstant.Test();
            string currentDirectory = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            //string currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine($"currentDirectory:{currentDirectory}");

            var config = new ConfigurationBuilder()
            .SetBasePath(currentDirectory)
            .AddJsonFile("hosting.json", optional: true)
            .Build();
            CreateWebHostBuilder(args, config, currentDirectory).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration config, string currentDirectory) =>
            WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(config)
                .UseContentRoot(currentDirectory)//定义contentroot  
                .UseWebRoot(Path.Combine(currentDirectory, "wwwroot"))//定义webroot
                                                                      //.UseUrls("http://*:12018", "https://*:22018")
                .UseStartup<Startup>();
    }
}
