using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services
{
    public interface IJPG2KService
    {
        IList<byte[]> ExtractContents(string filePath);
    }

    public class JPG2KService : IJPG2KService
    {
        private readonly IFileExtractService _fileExtractService;
        public JPG2KService(IFileExtractService fileExtractService) { 
            _fileExtractService = fileExtractService;
        }
        public IList<byte[]> ExtractContents(string filePath)
        {
            var magicSOF = new byte[] { 255, 79, 255, 81 };
            var magicEOF = new byte[] { 255, 217 };
        
            return _fileExtractService.ExtractContents(filePath, magicSOF, magicEOF);
        }
    }
}
