using System.IO.Compression;

namespace FileCarverNet.Services.Formats
{
    public interface IMP3Service
    {
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
    }

    public class MP3Service : BaseFileService, IMP3Service
    {
        public MP3Service(IStreamExtractService streamExtractService)
            : base(streamExtractService,
              new byte[] { 0x49, 0x44, 0x33 }, new byte[] { 0x0A, 0x65, 0x6E, 0x64, 0x73, 0x74, 0x72, 0x65, 0x61, 0x6D }, ".mp3")
        {

        }

        public override (bool withFile, byte[]? fileBytes) PostProcessing((bool withFile, byte[]? fileBytes) initialResults)
        {
            //return initialResults;

            if (initialResults.withFile == false) return initialResults;

            var tailBytes = 9;
            var startBytes = 10;

            for (var i = 2500; i > 1900; i--)
            {
                var b = initialResults.fileBytes.Skip(startBytes).Skip(i).Take(initialResults.fileBytes.Length - i - tailBytes - startBytes);

                try
                {
                    var s = new DeflateStream(new MemoryStream(b.ToArray()), CompressionMode.Decompress);
                    using (var o = new MemoryStream())
                    {
                        s.CopyTo(o);
                        var audioData = o.ToArray();

                        if (o.Length > 4096)
                        {
                            var combined = initialResults.fileBytes.Take(startBytes + i).Concat(audioData).ToArray();

                            return (true, combined);
                        }

                        //
                        //return (true, o.ToArray());
                    }
                }
                catch (Exception e)
                {

                }
            }

            return initialResults;
        }

        public bool IsZLib(IEnumerable<byte> bytes)
        {
            var data = bytes.ToArray();

            return data.Length > 2 &&
                   data[0] == 0x78 &&
                   (data[1] == 0x01 || data[1] == 0x9C || data[1] == 0xDA);
        }
    }
}
