namespace Domain.Commands
{
    public class LongResult
        : IResult<long>
    {
        private readonly long _size;

        public LongResult(long size)
        {
            _size = size;
        }

        public long GetResult()
        {
            return _size;
        }
    }
}