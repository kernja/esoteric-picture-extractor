namespace FileCarverNet.Services.Formats
{
    public interface IEMFService
    {
        (bool withFile, byte[]? fileBytes, string? extension) ProcessStream(int streamInteger);
    }

    public class EMFService : BaseFileService, IEMFService
    {
        public EMFService(IStreamExtractService streamExtractService)
            : base(streamExtractService,
              new byte[] { 1, 0, 0, 0, 116 }, new byte[] { 16, 0, 0, 0, 20, 0, 0, 0 }, ".emf")
        {
        }
    }
}
