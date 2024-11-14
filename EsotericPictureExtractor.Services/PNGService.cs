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
    }

    public class PNGService : IPNGService
    {
        private readonly IFileExtractService _fileExtractService;
        public PNGService(IFileExtractService fileExtractService) { 
            _fileExtractService = fileExtractService;
        }
        public IList<byte[]> ExtractContents(string filePath)
        {
            var magicSOF = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
            var magicEOF = new byte[] { 73, 69, 78, 68 };

            return _fileExtractService.ExtractContents(filePath, magicSOF, magicEOF);
        }
    }
}
