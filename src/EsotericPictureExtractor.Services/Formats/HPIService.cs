using SkiaSharp;

namespace FileCarverNet.Services.Formats
{
    public interface IHPIService
    {
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
    }

    public class HPIService : BaseFileService, IHPIService
    {
        private readonly IJFIFService _jfifService;
        private readonly IPNGService _pngService;

        public HPIService(IStreamExtractService streamExtractService,
            IJFIFService jfifService, IPNGService pngService)
            : base(streamExtractService,
              new byte[] { 137, 72, 80, 73 }, new byte[] { 73, 69, 78, 68 }, ".png", 4)
        {
            _jfifService = jfifService;
            _pngService = pngService;
        }
        public override (bool withFile, byte[]? fileBytes) PostProcessing((bool withFile, byte[]? fileBytes) initialResults)
        {
            if (initialResults.withFile == false) return initialResults;

            byte[]? jpgBytes = null;
            byte[]? pngBytes = null;

            foreach (var b in initialResults.fileBytes!)
            {
                var jpgResults = _jfifService.ProcessStream(b);
                var pngResults = _pngService.ProcessStream(b);

                if (jpgResults.withFile) jpgBytes = jpgResults.fileBytes!;
                if (pngResults.withFile) pngBytes = pngResults.fileBytes!;
            }

            if (jpgBytes != null)
            {
                var jpgImage = SKImage.FromEncodedData(jpgBytes);

                if (pngBytes != null)
                {
                    var pngImage = SKImage.FromEncodedData(pngBytes);
                    jpgImage = ApplyAlphaMask(jpgImage, pngImage);
                }

                return (true, jpgImage.Encode(SKEncodedImageFormat.Png, 100).ToArray());
            }
            else
            {
                throw new InvalidDataException("There is no JFIF (JPG) file within the data stream.");
            }
        }

        public static SKImage ApplyAlphaMask(SKImage jpgImage, SKImage pngImage)
        {
            // Convert from image to raw bitmap
            var jpg = SKBitmap.FromImage(jpgImage);
            var png = SKBitmap.FromImage(pngImage);

            // get the dimensions
            int width = jpg.Width;
            int height = jpg.Height;

            // Create the output canvas
            var result = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);

            // Get access to the raw pixels
            var jpgSpan = jpg.GetPixelSpan();
            var pngSpan = png.GetPixelSpan();
            var resSpan = result.GetPixelSpan();

            // Each pixel = 4 bytes (BGRA)
            for (int i = 0; i < width * height; i++)
            {
                int idx = i * 4;

                byte b = jpgSpan[idx];
                byte g = jpgSpan[idx + 1];
                byte r = jpgSpan[idx + 2];

                byte alpha = pngSpan[idx]; // grayscale -> take B (same as R/G since it's grayscale)

                resSpan[idx] = b;
                resSpan[idx + 1] = g;
                resSpan[idx + 2] = r;
                resSpan[idx + 3] = alpha;
            }

            return SKImage.FromBitmap(result);
        }
    }
}
