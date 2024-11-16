using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services.Formats
{
    public interface IGZIPService
    {
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
        (bool withFile, byte[]? fileBytes, string? extension) Flush();
    }

    public class GZIPService : BaseFileService, IGZIPService
    {
        public GZIPService(IStreamExtractService streamExtractService)
            : base(streamExtractService,
              new byte[] { 31, 139, 08, 00 }, new byte[] { 31, 139, 08, 00 }, ".gz", flushCheckEnd: false)
        {
        }

    }
}
