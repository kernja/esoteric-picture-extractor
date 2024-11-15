using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services.Formats
{
    public interface IWMFService
    {
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
    }

    public class WMFService : BaseFileService, IWMFService
    {
        public WMFService(IStreamExtractService streamExtractService)
            : base(streamExtractService,
              new byte[] { 215, 205, 198, 154 }, new byte[] { 3, 0, 0, 0, 0, 0 }, ".wmf")
        {
        }
    }
}
