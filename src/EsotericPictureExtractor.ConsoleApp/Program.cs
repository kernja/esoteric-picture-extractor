using EsotericPictureExtractor.Services;
using EsotericPictureExtractor.Services.Formats;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EsotericPictureExtractor.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting program...");
            var services = CreateServices();

            var jpg2kService = services.GetRequiredService<IJPG2KService>();
            var bz2Service = services.GetRequiredService<IBZ2Service>();
            var pngService = services.GetRequiredService<IPNGService>();
            var jfifService = services.GetRequiredService<IJFIFService>();
            var hpiService = services.GetRequiredService<IHPIService>();
            var gzipService = services.GetRequiredService<IGZIPService>();
            var wmfService = services.GetRequiredService<IWMFService>();
            var emfService = services.GetRequiredService<IEMFService>();
            var fileSystemService = services.GetRequiredService<IIOService>();

            var sourceFile = "C:\\Users\\Jeff\\source\\repos\\EsotericPictureExtractor\\testFiles\\combinedFiles.dat";
            var outputFolder = "C:\\Users\\Jeff\\source\\repos\\EsotericPictureExtractor\\testFiles\\";

            var file = new IOService();
            using (var s = file.GetStream(sourceFile))
            {
                int count = 1;
                int b;
                b = s.ReadByte();
                while (b >= 0)
                {
                    var result = wmfService.ProcessStream(b);
                    if (result.withFile == true)
                    {
                        file.WriteBinary($"{outputFolder}testStream_{count}{result.extension!}", result.fileBytes!);
                        count++;
                    }

                    b = s.ReadByte();
                    
                }

            }
        }

        private static ServiceProvider CreateServices()
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .Build();

            var serviceProvider = new ServiceCollection()
                .AddTransient<IIOService, IOService>()
                .AddTransient<IJPG2KService, JPG2KService>()
                .AddTransient<IBZ2Service, BZ2Service>()
                .AddTransient<IPNGService, PNGService>()
                .AddTransient<IJFIFService, JFIFService>()
                .AddTransient<IHPIService, HPIService>()
                .AddTransient<IGZIPService, GZIPService>()
                .AddTransient<IWMFService, WMFService>()
                .AddTransient<IEMFService, EMFService>()
                .AddTransient<IStreamExtractService, StreamExtractService>()
                .AddSingleton<IConfiguration>(configuration);

            return serviceProvider.BuildServiceProvider();
        }
    }
}
