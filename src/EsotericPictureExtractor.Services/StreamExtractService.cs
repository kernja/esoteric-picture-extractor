namespace EsotericPictureExtractor.Services
{
    public interface IStreamExtractService
    {
        void ConfigureStream(byte[] magicSOF, byte[] magicEOF, int overRead = 0, bool flushCheckEnd = false);
        (bool withFile, byte[]? fileBytes) ProcessStream(int streamInteger);
        (bool withFile, byte[]? fileBytes) Flush();

    }
    public class StreamExtractService : IStreamExtractService
    {
        byte[] _magicSOF = new byte[0];
        byte[] _magicEOF = new byte[0];
        int _overRead;

        IList<byte> _fileBytes = new List<byte>();

        int _minBufferLength;
        int _startLength;
        int _endLength;

        bool _flushCheckEnd = true;

        bool _initialized = false;

        public void ConfigureStream(byte[] magicSOF, byte[] magicEOF, int overRead = 0, bool flushCheckEnd = true)
        {
            _magicSOF = magicSOF;
            _startLength = _magicSOF.Length;

            _magicEOF = magicEOF;
            _endLength = _magicEOF.Length + overRead;

            _overRead = overRead;
            _minBufferLength = _startLength + _endLength;

            _flushCheckEnd = flushCheckEnd;

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
            if (CompareArrayValues(_magicSOF, _fileBytes.Take(_startLength).ToList()) == false)
            {
                _fileBytes.RemoveAt(0);
                return (false, null);
            }
            else
            {
                if (CompareArrayValues(_magicEOF, _fileBytes.TakeLast(_endLength).ToList(), _overRead) == true)
                {
                    var isolatedFile = _fileBytes.ToArray();

                    _fileBytes.Clear();
                    if (_magicSOF.SequenceEqual(_magicEOF)) _fileBytes = _magicSOF.ToList();
                    return (true, isolatedFile);

                }
                else
                {
                    return (false, null);
                }
            }
        }

        public (bool withFile, byte[]? fileBytes) Flush()
        {
            if (CompareArrayValues(_magicSOF, _fileBytes.Take(_startLength).ToList()) == false) return (false, null);
            if (CompareArrayValues(_magicEOF, _fileBytes.TakeLast(_endLength).ToList(), _overRead) == false && _flushCheckEnd == true) return (false, null);

            return (true, _fileBytes.ToArray());
        }
        bool CompareArrayValues(byte[] pA, IList<byte> pB, int overRead = 0)
        {
            var b = pB.SkipLast(overRead).ToArray();
            return pA.SequenceEqual(b);
        }
    }
}
