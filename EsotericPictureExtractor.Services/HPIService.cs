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
        //IList<byte[]> ExtractContents(string filePath);
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
    }

    public class HPIService : ImageService, IHPIService
    {
        private readonly IJFIFService _jfifService;
        private readonly IPNGService _pngService;

        public HPIService(IFileExtractService fileExtractService, IFileStreamExtractService streamExtractService,
            IJFIFService jfifService, IPNGService pngService)
            : base(fileExtractService, streamExtractService,
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
                throw new InvalidDataException("There is no JFIF file within the data stream.");
            }
        }
        /*
        public IList<byte[]> ExtractContents(string filePath)
        {
            var jBytes = _jfifService.ExtractContents(filePath);
            var pBytes = _pngService.ExtractContents(filePath);

            var results = new List<byte[]>();
            
            if (jBytes.Count == 1 && pBytes.Count == 1)
            {
                var pByte = pBytes[0];
                Array.Resize<byte>(ref pByte, pByte.Length + 4);
                MagickImage jImage = new MagickImage(jBytes[0]);
                MagickImage pImage = new MagickImage(pByte);

                jImage.Composite(pImage, CompositeOperator.CopyAlpha);
                results.Add(jImage.ToByteArray(MagickFormat.Png));
            } else if (jBytes.Count == 1)
            {
                results.Add(jBytes[0]);
            }

            return results;
        }*/
    }
}
