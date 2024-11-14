using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services
{
    public interface IBZ2Service
    {
        IList<byte[]> ExtractContents(string filePath);
    }

    public class BZ2Service : IBZ2Service
    {
        private readonly IFileExtractService _fileExtractService;
        public BZ2Service(IFileExtractService fileExtractService)
        {
            _fileExtractService = fileExtractService;
        }
        public IList<byte[]> ExtractContents(string filePath)
        {
            var magicSOF = new byte[] { 66, 90, 104, 57 };
            var magicEOF = new byte[] { 69, 56, 80, 144 };

            return _fileExtractService.ExtractContents(filePath, magicSOF, magicEOF);
        }
    }
}
