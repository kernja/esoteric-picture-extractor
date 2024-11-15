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
        void ExtractStream(string filePath);
    }

    public class JPG2KService : IJPG2KService
    {
        private readonly IFileExtractService _fileExtractService;
        private readonly IFileStreamExtractService _streamExtractService;
        public JPG2KService(IFileExtractService fileExtractService, IFileStreamExtractService streamExtractService) { 
            _fileExtractService = fileExtractService;
            _streamExtractService = streamExtractService;

            _streamExtractService.ConfigureStream(new byte[] { 255, 79, 255, 81 }, new byte[] { 255, 217 });
        }
        public IList<byte[]> ExtractContents(string filePath)
        {
            var magicSOF = new byte[] { 255, 79, 255, 81 };
            var magicEOF = new byte[] { 255, 217 };
        
            return _fileExtractService.ExtractContents(filePath, magicSOF, magicEOF);
        }


        public void ExtractStream(string filePath)
        {
            var file = new FileSystemService();
            
            using (var s = file.GetStream(filePath))
            {
                int b;
                b = s.ReadByte();
                while (b >= 0)
                {

                    var result = _streamExtractService.ProcessStream(b);

                    if (result.withFile == true)
                    {
                        file.WriteBinary("./testStream.jp2", result.fileBytes);
                    }

                    b = s.ReadByte();
                }

            }
        }

    }
}
