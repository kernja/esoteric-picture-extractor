using EsotericPictureExtractor.Services;
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
            var hPIService = services.GetRequiredService<IHPIService>();
            var gzipService = services.GetRequiredService<IGZIPService>();
            var fileSystemService = services.GetRequiredService<IFileSystemService>();

            var sourceFile = "C:\\Program Files (x86)\\Cosmi\\Print Perfect Clip Art\\Content\\Raster0_CNT.dat";
            var outputFolder = "C:\\Users\\Jeff\\OneDrive\\Dropbox\\ax5\\CosmiClip\\Raster0b_CNT\\";

            jpg2kService.ExtractStream(sourceFile);
            /*
            var extractedFiles = gzipService.ExtractContents(sourceFile);

            var count = 1;
            if (extractedFiles.Count > 0)
            {
                foreach (var f in extractedFiles)
                {
                    fileSystemService.WriteBinary($"{outputFolder}{count}.gzip", f);
                    count++;
                }
            }*/
        }

        private static ServiceProvider CreateServices()
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .Build();

            var serviceProvider = new ServiceCollection()
                .AddTransient<IFileSystemService, FileSystemService>()
                .AddTransient<IFileExtractService, FileExtractService>()
                .AddTransient<IJPG2KService, JPG2KService>()
                .AddTransient<IBZ2Service, BZ2Service>()
                .AddTransient<IPNGService, PNGService>()
                .AddTransient<IJFIFService, JFIFService>()
                .AddTransient<IHPIService, HPIService>()
                .AddTransient<IGZIPService, GZIPService>()
                .AddTransient<IFileStreamExtractService, FileStreamExtractService>()
                .AddSingleton<IConfiguration>(configuration);

            return serviceProvider.BuildServiceProvider();
        }
    }
}
