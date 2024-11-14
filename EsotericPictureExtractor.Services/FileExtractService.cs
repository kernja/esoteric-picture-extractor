using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsotericPictureExtractor.Services
{
    public interface IFileExtractService
    {
        IList<byte[]> ExtractContents(byte[] sourceData, byte[] magicSOF, byte[] magicEOF);
        IList<byte[]> ExtractContents(string filePath, byte[] magicSOF, byte[] magicEOF);

    }
    public class FileExtractService : IFileExtractService
    {
        private readonly IFileSystemService _fileSystem;
        public FileExtractService(IFileSystemService fileSystem)
        {
            _fileSystem = fileSystem;

            if (_fileSystem == null) throw new InvalidOperationException("IFileSystemService cannot be null.");
        }

        public IList<byte[]> ExtractContents(string filePath, byte[] magicSOF, byte[] magicEOF)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentOutOfRangeException(nameof(filePath));
            return this.ExtractContents(_fileSystem.ReadBinary(filePath), magicSOF, magicEOF);

        }
        public IList<byte[]> ExtractContents(byte[] sourceData, byte[] magicSOF, byte[] magicEOF)
        {
            if (sourceData == null) throw new ArgumentNullException(nameof(sourceData));
            if (magicSOF == null) throw new ArgumentNullException(nameof(magicSOF));
            if (magicEOF == null) throw new ArgumentNullException(nameof(magicEOF));

            var delimiter = ',';
            var results = new List<byte[]>();
            
           
                var sofString = string.Join(delimiter, magicSOF.Select(x => x.ToString()).ToArray());
                var eofString = string.Join(delimiter, magicEOF.Select(x => x.ToString()).ToArray());
                var fileString = string.Join(delimiter, sourceData.Select(x => x.ToString()).ToArray());

                var fileArray = fileString.Split(sofString);

                //remove first result
                if (fileArray.Length > 0)
                {
                    fileArray = fileArray.Skip(1).ToArray();

                    foreach (var f in fileArray)
                    {
                        try
                        {
                            var innerString = f.Split(eofString)[0];
                            innerString = $"{sofString}{innerString}{eofString}";
                            innerString = innerString.TrimEnd(',');

                            using (MemoryStream ms = new MemoryStream(innerString.Split(delimiter).Where(x => string.IsNullOrWhiteSpace(x) == false).Select(x => byte.Parse(x)).ToArray()))
                            {
                                ms.Position = 0;
                                results.Add(ms.ToArray());
                            }
                        }
                        catch (Exception ex)
                        {
                            //throw new Exception("Unable to extract file contents, see inner exception.", ex);
                        }
                    }
                }
            

            return results;
        }
    }
}
