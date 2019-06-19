using System;
using System.IO;
using System.Threading.Tasks;

namespace Domain.Commands
{
    public class DownloadFileCommand
        : IDownloadFileCommand<FileDownloadResult, Stream, DownloadFileState>
    {
        public async Task<FileDownloadResult> ExecuteAsync(DownloadFileState state)
        {
            return await Task.Run(() => Execute(state));
        }

        public FileDownloadResult Execute(DownloadFileState state)
        {
            // This solution may result in OutOfMemory exception
            // for big files. Other solutions may be used instead
            // (e.g. Response.TransmitFile, manual chunking, setting OutputBuffer size to 0 etc.

            try
            {
                var memory = new MemoryStream();

                using (var stream = new FileStream(state.Target.Raw, FileMode.Open))
                {
                    stream.CopyTo(memory);
                }

                memory.Position = 0;

                Result = new FileDownloadResult(true, memory);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                Result = new FileDownloadResult(false, null);
            }

            return Result;
        }

        public Stream GetResult()
        {
            return (MemoryStream) Result.Result;
        }

        public FileDownloadResult Result { get; private set; }
    }
}