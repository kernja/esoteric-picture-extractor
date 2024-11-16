using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services
{

    public abstract class BaseFileService 
    {
        protected readonly IStreamExtractService _streamExtractService;

        protected byte[] _magicSOF = new byte[0]; 
        protected byte[] _magicEOF = new byte[0];
        protected int _overRead = 0;
        protected string _fileExtension = "";
        protected bool _flushCheckEnd = true;

        public BaseFileService(IStreamExtractService streamExtractService, byte[] magicSOF, byte[] magicEOF, string fileExtension, int overRead = 0, bool flushCheckEnd = true) { 
            _streamExtractService = streamExtractService;

            _magicSOF = magicSOF;
            _magicEOF = magicEOF;
            _overRead = overRead;
            _fileExtension = fileExtension;
            _flushCheckEnd = flushCheckEnd;

            _streamExtractService.ConfigureStream(_magicSOF, _magicEOF, _overRead);
        }

        public (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger)
        {
            var result = _streamExtractService.ProcessStream(streamInteger);
            if (result.withFile == false)
                return (false, null, null);

            result = PostProcessing(result);
            if (result.withFile == false)
                return (false, null, null);

            return (true, result.fileBytes, _fileExtension);
        }


        public virtual (bool withFile, byte[]? fileBytes, string? extension) Flush()
        {
            var result = _streamExtractService.Flush();
            if (result.withFile == false)
                return (false, null, null);

            result = PostProcessing(result);
            if (result.withFile == false)
                return (false, null, null);

            return (true, result.fileBytes, _fileExtension);
        }

        public virtual (bool withFile, byte[]? fileBytes) PostProcessing((bool withFile, byte[]? fileBytes) file)
        {
            return file;
        }
    }
}
