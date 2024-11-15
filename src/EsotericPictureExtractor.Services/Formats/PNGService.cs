using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services.Formats
{
    public interface IPNGService
    {
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
    }

    public class PNGService : BaseFileService, IPNGService
    {
        public PNGService(IStreamExtractService streamExtractService)
            : base(streamExtractService,
              new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }, new byte[] { 73, 69, 78, 68 }, ".png", 4)
        {
        }
    }
}
