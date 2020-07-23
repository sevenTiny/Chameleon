using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Chameleon.DataApi
{
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
                    webBuilder.UseStartup<Startup>().UseUrls("http://*:39011")
                    //解除文件上传大小限制，iis部署模式下要删除
                    .UseKestrel(options =>
                    {
                        options.Limits.MaxRequestBodySize = long.MaxValue;
                        options.Limits.MaxRequestBufferSize = long.MaxValue;
                        options.Limits.MaxRequestLineSize = int.MaxValue;
                    });
                });
    }
}
