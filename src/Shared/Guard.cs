using System.Diagnostics.CodeAnalysis;

namespace Shared;

public static class Guard
{
    public static void NotNull([NotNull] object? obj, string paramName)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}
