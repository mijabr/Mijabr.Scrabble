using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using words;

namespace Mijabr.Scrabble
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            LoadDictionary(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

        private static void LoadDictionary(IHost host)
        {
            var dictionary = (WordDictionary)host.Services.GetService(typeof(WordDictionary));
#if DEBUG
            dictionary.LoadFile(@"dictionary.txt");
#else
            dictionary.LoadFile("dictionary.txt");
#endif
        }
    }
}
