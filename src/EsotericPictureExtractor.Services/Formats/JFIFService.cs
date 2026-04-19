namespace FileCarverNet.Services.Formats
{
    public interface IJFIFService
    {
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
    }

    public class JFIFService : BaseFileService, IJFIFService
    {
        public JFIFService(IStreamExtractService streamExtractService)
            : base(streamExtractService,
              new byte[] { 255, 216, 255 }, new byte[] { 255, 217 }, ".jpg")
        {
        }

    }
}
