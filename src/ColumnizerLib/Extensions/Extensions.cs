using ColumnizerLib;

namespace ColumnizerLib.Extensions;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Intentionally")]
public static class Extensions
{
    /// <summary>
    /// 修复：使用标准扩展方法语法，兼容所有VS版本
    /// </summary>
    public static string ToClipBoardText (this ILogLineMemory logLine)
    {
        return logLine == null ? string.Empty : $"\t{logLine.LineNumber + 1}\t{logLine.FullLine}";
    }
}