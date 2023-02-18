using System.Threading.Tasks;

namespace WorkerHost.Extensions
{
    internal static class TaskExtensions
    {
        public static Task<T> AsTask<T>(this T obj)
        {
            return Task.FromResult(obj);
        }

        public static Task<T?> AsNullable<T>(this Task<T> task)
        {
            return task!;
        }
    }
}
