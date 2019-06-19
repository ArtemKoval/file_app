namespace Domain.Commands
{
    public class FileDownloadResult: IResult
    {
        public FileDownloadResult(bool success,
            object result)
        {
            Success = success;
            Result = result;
        }

        public bool Success { get; }
        public object Result { get; }
    }
}