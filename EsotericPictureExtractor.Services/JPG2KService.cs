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
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
    }

    public class JPG2KService : ImageService, IJPG2KService
    {
        public JPG2KService(IFileExtractService fileExtractService, IFileStreamExtractService streamExtractService) 
            : base(fileExtractService, streamExtractService,
              new byte[] { 255, 79, 255, 81 }, new byte[] { 255, 217 }, ".jp2") { 
        }

        public IList<byte[]> ExtractContents(string filePath)
        {
            var magicSOF = new byte[] { 255, 79, 255, 81 };
            var magicEOF = new byte[] { 255, 217 };
        
            return _fileExtractService.ExtractContents(filePath, magicSOF, magicEOF);
        }

    }
}
