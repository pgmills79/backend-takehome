using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace RatingEngine
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); webBuilder.UseUrls("http://localhost:80");});



    }
    
   
}