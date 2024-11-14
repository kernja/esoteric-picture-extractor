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
            var fileSystemService = services.GetRequiredService<IFileSystemService>();
            var files = bz2Service.ExtractContents("C:\\Program Files (x86)\\Cosmi\\Print Perfect Clip Art\\Content\\Vector0_CNT.dat");
            //var files = jpg2kService.ExtractContents("C:\\Program Files (x86)\\Cosmi\\Print Perfect Clip Art\\Content\\Raster0_CNT.dat");

            if (files.Count > 0)
            {
                fileSystemService.WriteBinary(".\\test.bz2", files.First());
                //fileSystemService.WriteBinary(".\\test.jp2", files.First());
            }
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
                .AddSingleton<IConfiguration>(configuration);

            return serviceProvider.BuildServiceProvider();
        }
    }
}
