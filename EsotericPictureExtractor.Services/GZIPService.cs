using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services
{
    public interface IGZIPService
    {
        IList<byte[]> ExtractContents(string filePath);
    }

    public class GZIPService : IGZIPService
    {
        private readonly IFileExtractService _fileExtractService;
        public GZIPService(IFileExtractService fileExtractService) { 
            _fileExtractService = fileExtractService;
        }
        public IList<byte[]> ExtractContents(string filePath)
        {
            var magicSOF = new byte[] { 31, 139, 08, 00 };

            return _fileExtractService.ExtractContents(filePath, magicSOF, magicSOF, 0);
        }
    }
}
