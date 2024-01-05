using System.Linq;

namespace Shared;

public static class StringExtensions
{
    public static string AlignFromFirstLine(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        if (!source.StartsWith("\r\n", StringComparison.OrdinalIgnoreCase))
        {
            throw new FormatException("String must start with a NewLine character.");
        }

        var indentationSize = source.Skip("\r\n".Length)
            .TakeWhile(char.IsWhiteSpace)
            .Count();

        var indentationStr = new string(' ', indentationSize);
        return source.TrimStart().Replace($"\n{indentationStr}", "\n");
    }
}
