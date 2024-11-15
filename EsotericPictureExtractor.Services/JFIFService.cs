using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services
{
    public interface IJFIFService
    {
        IList<byte[]> ExtractContents(string filePath);
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
    }

    public class JFIFService : ImageService, IJFIFService
    {
        public JFIFService(IFileExtractService fileExtractService, IFileStreamExtractService streamExtractService)
            : base(fileExtractService, streamExtractService,
              new byte[] { 255, 216, 255 }, new byte[] { 255, 217 }, ".jpg")
        {
        }
        public IList<byte[]> ExtractContents(string filePath)
        {
            var magicSOF = new byte[] { 255, 216, 255 };
            var magicEOF = new byte[] { 255, 217 };
        
            return _fileExtractService.ExtractContents(filePath, magicSOF, magicEOF);
        }
    }
}
