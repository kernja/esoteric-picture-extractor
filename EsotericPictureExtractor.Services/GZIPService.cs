using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services
{
    public interface IGZIPService
    {
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
    }

    public class GZIPService : BaseFileService, IGZIPService
    {
        public GZIPService(IStreamExtractService streamExtractService)
            : base(streamExtractService,
              new byte[] { 31, 139, 08, 00 }, new byte[] { 31, 139, 08, 00 }, ".gz")
        {
        }
    }
}
