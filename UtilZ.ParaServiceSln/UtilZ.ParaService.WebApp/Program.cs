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
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())//定义contentroot  
                .UseWebRoot(Directory.GetCurrentDirectory() + "/wwwroot")//定义webroot
                                                                         //.UseUrls("http://localhost:5001", "https://localhost:5002", "http://*:5003", "https://*:5004")
                //                                                             .UseSetting("https_port", "5004")
                //.UseUrls("http://*:5003")
                .UseStartup<Startup>();
    }
}
