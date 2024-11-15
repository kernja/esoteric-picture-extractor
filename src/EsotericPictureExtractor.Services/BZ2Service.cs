using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services
{
    public interface IBZ2Service
    {
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
    }

    public class BZ2Service : BaseFileService, IBZ2Service
    {
        public BZ2Service(IStreamExtractService streamExtractService)
            : base(streamExtractService,
              new byte[] { 66, 90, 104, 57 }, new byte[] { 69, 56, 80, 144 }, ".bz2")
        {

        }
    }
}
