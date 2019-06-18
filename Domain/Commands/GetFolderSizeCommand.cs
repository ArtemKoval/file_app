using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Commands
{
    public class GetFolderSizeCommand
        : IGetFolderSizeCommand<long>
    {
        private static long GetDirectorySize(DirectoryInfo directoryInfo)
        {
            var fileInfos = directoryInfo
                .GetFiles();
            var size = fileInfos
                .Sum(fi => fi.Length);

            var directoryInfos = directoryInfo
                .GetDirectories();
            size += directoryInfos
                .Sum(di => GetDirectorySize(di));

            return size;
        }

        public async Task<IResult<long>> ExecuteAsync(CommandState state)
        {
            return await Task.Run(() => Execute(state));
        }

        public IResult<long> Execute(CommandState state)
        {
            long size = 0;

            try
            {
                var directoryInfo = new DirectoryInfo(state.Target);

                size += GetDirectorySize(directoryInfo);

                return new LongResult(size);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return new LongResult(0L);
            }
        }
    }
}