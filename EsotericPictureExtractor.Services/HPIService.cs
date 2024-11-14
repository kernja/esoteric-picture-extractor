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
        IList<byte[]> ExtractContents(string filePath);
    }

    public class HPIService : IHPIService
    {
        private readonly IJFIFService _jfifService;
        private readonly PNGService _pngService;
        public HPIService(IJFIFService jfifService, PNGService pngService) {
            _jfifService = jfifService;
            _pngService = pngService;
        }
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
        }
    }
}
