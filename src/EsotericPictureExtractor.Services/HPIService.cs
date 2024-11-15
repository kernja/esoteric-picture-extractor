using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services
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
                MagickImage jpgImage = new MagickImage(jpgBytes);

                if (pngBytes != null)
                {
                    MagickImage pImage = new MagickImage(pngBytes);
                    jpgImage.Composite(pImage, CompositeOperator.CopyAlpha);
                }

                return (true, jpgImage.ToByteArray(MagickFormat.Png));
            } else
            {
                throw new InvalidDataException("There is no JFIF (JPG) file within the data stream.");
            }
        }
    }
}
