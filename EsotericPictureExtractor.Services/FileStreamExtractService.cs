namespace EsotericPictureExtractor.Services
{
    public interface IFileStreamExtractService
    {
        void ConfigureStream(byte[] magicSOF, byte[] magicEOF, int overRead = 0);
        (bool withFile, byte[]? fileBytes) ProcessStream(int streamInteger);

    }
    public class FileStreamExtractService : IFileStreamExtractService
    {
        byte[] _magicSOF = new byte[0];
        byte[] _magicEOF = new byte[0];
        int _overRead;

        IList<byte> _fileBytes = new List<byte>();

        int _minBufferLength;
        int _startLength;
        int _endLength;

        bool _initialized = false;

        public void ConfigureStream(byte[] magicSOF, byte[] magicEOF, int overRead = 0)
        {
            _magicSOF = magicSOF;
            _startLength = _magicSOF.Length;

            _magicEOF = magicEOF;
            _endLength = _magicEOF.Length + overRead;

            _overRead = overRead;
            _minBufferLength = _startLength + _endLength;

            _fileBytes = new List<byte>();
            _initialized = true;
        }
        public (bool withFile, byte[]? fileBytes) ProcessStream(int streamInteger)
        {
            // Need To configure the stream before we can process it
            if (_initialized == false) throw new InvalidOperationException();

            byte sB = Convert.ToByte(streamInteger);

            _fileBytes.Add(sB);
            if (_fileBytes.Count < _minBufferLength) return (false, null);

            // Pop the first item in the list if the magic bytes don't align
            if (CompareArrayValues(_magicSOF, _fileBytes.Take(_startLength).ToList(), 0) == false)
            {
                _fileBytes.RemoveAt(0);
                return (false, null);
            }
            else
            {
                if (CompareArrayValues(_magicEOF, _fileBytes.TakeLast(_endLength).ToList(), _overRead) == true)
                {
                    var isolatedFile = _fileBytes.ToArray();

                    if (_magicSOF.SequenceEqual(_magicEOF)) _fileBytes = _magicSOF.ToList();
                    return (true, isolatedFile);

                }
                else
                {
                    return (false, new byte[0]);
                }
            }
        }

        bool CompareArrayValues(byte[] pA, IList<byte> pB, int overRead)
        {
            var b = pB.SkipLast(overRead).ToArray();
            return pA.SequenceEqual(b);
        }
    }
}
