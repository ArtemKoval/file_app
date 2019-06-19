namespace Domain.Commands
{
    public class FileUploadResult : IResult
    {
        public FileUploadResult(bool success,
         object result)
        {
            Success = success;
            Result = result;
        }

        public bool Success { get; }
        public object Result { get; }
    }
}