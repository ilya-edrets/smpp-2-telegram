#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
#pragma warning disable RCS1175 // Unused 'this' parameter.
#pragma warning disable IDE0060 // Remove unused parameter

namespace Shared;

public static class TaskExtensions
{
    public static Task<T> AsTask<T>(this T obj)
    {
        return Task.FromResult(obj);
    }

    public static Task<T?> AsNullable<T>(this Task<T> task)
    {
        return task!;
    }

    public static void Forget<T>(this Task<T> task)
    {
    }

    public static void Forget(this Task task)
    {
    }
}
