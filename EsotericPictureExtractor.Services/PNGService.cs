using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services
{
    public interface IPNGService
    {
        IList<byte[]> ExtractContents(string filePath);
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
    }

    public class PNGService : ImageService, IPNGService
    {
        public PNGService(IFileExtractService fileExtractService, IFileStreamExtractService streamExtractService)
            : base(fileExtractService, streamExtractService,
              new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }, new byte[] { 73, 69, 78, 68 }, ".png", 4)
        {
        }

        public IList<byte[]> ExtractContents(string filePath)
        {
            var magicSOF = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
            var magicEOF = new byte[] { 73, 69, 78, 68 };

            return _fileExtractService.ExtractContents(filePath, magicSOF, magicEOF, 4);
        }
    }
}
