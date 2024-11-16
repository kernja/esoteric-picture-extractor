using EsotericPictureExtractor.Services;
using EsotericPictureExtractor.Services.Formats;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace EsotericPictureExtractor.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var arguments = ProcessArgs(args);
            
            var services = CreateServices();
            var jpg2kService = services.GetRequiredService<IJPG2KService>();
            var bz2Service = services.GetRequiredService<IBZ2Service>();
            var pngService = services.GetRequiredService<IPNGService>();
            var jfifService = services.GetRequiredService<IJFIFService>();
            var hpiService = services.GetRequiredService<IHPIService>();
            var gzipService = services.GetRequiredService<IGZIPService>();
            var wmfService = services.GetRequiredService<IWMFService>();
            var emfService = services.GetRequiredService<IEMFService>();
            var io = services.GetRequiredService<IIOService>();

            var sourceFile = arguments.sourceFile;
            var outputFolder = arguments.targetDirectory;

            using (var s = io.GetStream(sourceFile))
            {
              
                int b;
                b = s.ReadByte();
                while (b >= 0)
                {
                    if (arguments.mode.Contains("WMF")) ProcessStreams(io, arguments.targetDirectory, wmfService.ProcessStream(b));
                    if (arguments.mode.Contains("EMF")) ProcessStreams(io, arguments.targetDirectory, emfService.ProcessStream(b));
                    if (arguments.mode.Contains("GZIP")) ProcessStreams(io, arguments.targetDirectory, gzipService.ProcessStream(b));
                    if (arguments.mode.Contains("HPI")) ProcessStreams(io, arguments.targetDirectory, hpiService.ProcessStream(b));
                    if (arguments.mode.Contains("JFIF")) ProcessStreams(io, arguments.targetDirectory, jfifService.ProcessStream(b));
                    if (arguments.mode.Contains("PNG")) ProcessStreams(io, arguments.targetDirectory, pngService.ProcessStream(b));
                    if (arguments.mode.Contains("BZ2")) ProcessStreams(io, arguments.targetDirectory, bz2Service.ProcessStream(b));
                    if (arguments.mode.Contains("JP2")) ProcessStreams(io, arguments.targetDirectory, jpg2kService.ProcessStream(b));

                    b = s.ReadByte();
                }

            }

            if (arguments.mode.Contains("GZIP")) ProcessStreams(io, arguments.targetDirectory, gzipService.Flush());
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

        private static (string sourceFile, string targetDirectory, string mode)  ProcessArgs(string[] args)
        {
            if (args.Length != 3) throw new Exception("Invalid number of arguments.");
            return (args[0], args[1], args[2].ToUpper());
        }

        private static void ProcessStreams(IIOService ioService, string outputFolder, (bool withFile, byte[]? fileBytes, string? extension) result)
        {
            if (result.withFile == false) return;

            ioService.WriteBinary($"{outputFolder}\\{DateTime.Now.ToString("MM-dd-yyyy-hh-mm-ss-f")}{result.extension!}", result.fileBytes!);
        }
    }
}
