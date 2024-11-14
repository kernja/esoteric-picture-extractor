using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services
{
    public interface IJFIFService
    {
        IList<byte[]> ExtractContents(string filePath);
    }

    public class JFIFService : IJFIFService
    {
        private readonly IFileExtractService _fileExtractService;
        public JFIFService(IFileExtractService fileExtractService) { 
            _fileExtractService = fileExtractService;
        }
        public IList<byte[]> ExtractContents(string filePath)
        {
            var magicSOF = new byte[] { 255, 216, 255 };
            var magicEOF = new byte[] { 255, 217 };
        
            return _fileExtractService.ExtractContents(filePath, magicSOF, magicEOF);
        }
    }
}
