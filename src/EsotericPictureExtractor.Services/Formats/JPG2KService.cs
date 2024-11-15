using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services.Formats
{
    public interface IJPG2KService
    {
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
    }

    public class JPG2KService : BaseFileService, IJPG2KService
    {
        public JPG2KService(IStreamExtractService streamExtractService)
            : base(streamExtractService,
              new byte[] { 255, 79, 255, 81 }, new byte[] { 255, 217 }, ".jp2")
        {
        }

    }
}
