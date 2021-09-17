using ASPNET_WebAPI_Testing.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNET_WebAPI_Testing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string json = File.ReadAllText(@"appsettings.json");
            JObject o = JObject.Parse(@json);
            AppSettings.appSettings = JsonConvert.DeserializeObject<AppSettings>(o["AppSettings"].ToString());

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();
    }
}