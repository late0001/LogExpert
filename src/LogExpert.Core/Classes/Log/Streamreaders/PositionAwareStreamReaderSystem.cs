using System.Text;

using LogExpert.Core.Classes.Log.Buffers;
using LogExpert.Core.Entities;
using LogExpert.Core.Interfaces;

namespace LogExpert.Core.Classes.Log.Streamreaders;

/// <summary>
/// This class is responsible for reading line from the log file. It also decodes characters with the appropriate charset encoding.
/// PositionAwareStreamReaderSystem tries a BOM detection to determine correct file offsets when directly seeking into the file (on re-loading flushed buffers).
/// UTF-8 handling is a bit slower, because after reading a character the byte length of the character must be determined.
/// Lines are read char-by-char. StreamReader.ReadLine() is not used because StreamReader cannot tell a file position.
/// </summary>
public class PositionAwareStreamReaderSystem : PositionAwareStreamReaderBase, ILogStreamReaderMemory
{
    #region Fields

    private const int CHAR_CR = 0x0D;
    private const int CHAR_LF = 0x0A;

    private int _newLineSequenceLength;

    public override bool IsDisposed { get; protected set; }

    #endregion

    #region cTor

    public PositionAwareStreamReaderSystem (Stream stream, EncodingOptions encodingOptions, int maximumLineLength) : base(stream, encodingOptions, maximumLineLength)
    {
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or creates the block allocator used by this reader instance.
    /// The caller can detach the blocks after reading a buffer's worth of lines.
    /// </summary>
	private CharBlockAllocator _blockAllocator;
    public CharBlockAllocator BlockAllocator
    {
        get => _blockAllocator ??= new CharBlockAllocator();
        private set => _blockAllocator = value;
    }

    #endregion

    #region Public methods

    public override string ReadLine ()
    {
        var reader = GetStreamReader();

        if (_newLineSequenceLength == 0)
        {
            _newLineSequenceLength = GuessNewLineSequenceLength(reader);
        }

        var line = reader.ReadLine();

        if (line != null)
        {
            MovePosition(Encoding.GetByteCount(line) + _newLineSequenceLength);

            if (line.Length > MaximumLineLength)
            {
                line = line[..MaximumLineLength];
            }
        }

        return line;
    }

    /// <summary>
    /// Attempts to read the next line from the stream without allocating a new string.
    /// The returned Memory&lt;char&gt; is valid until the next call to TryReadLine or ReturnMemory.
    /// </summary>
    public bool TryReadLine (out ReadOnlyMemory<char> lineMemory)
    {
        lineMemory = default; // 初始化默认值，避免未赋值
        var reader = GetStreamReader();

        try
        {
            if (_newLineSequenceLength == 0)
            {
                _newLineSequenceLength = GuessNewLineSequenceLength(reader);
            }

            // 读取行（不含换行符），并记录原始行长度
            var originalLine = reader.ReadLine();
            if (originalLine is null)
            {
                return false;
            }

            // 修正1：计算实际截取的字符长度，并仅累加截取部分的字节数
            var actualLength = Math.Min(originalLine.Length, MaximumLineLength);
            var lineBytes = Encoding.GetByteCount(originalLine.AsSpan(0, actualLength));
            // 修正2：Position 仅累加 有效行字节数 + 换行符字节数（避免重复计算）
            MovePosition(lineBytes + _newLineSequenceLength);

            // 修正3：安全分配内存，处理分配失败场景
            var allocator = BlockAllocator;
            if (!allocator.TryRent(actualLength, out var target)) // 新增 TryRent 方法（见下文）
            {
                return false;
            }

            // 复制截取后的字符到分配的内存
            originalLine.AsSpan(0, actualLength).CopyTo(target.Span);
            lineMemory = target;
            return true;
        }
        catch (IOException)
        {
            // 流读取异常，返回 false 符合 TryXXX 语义
            return false;
        }
        catch (ObjectDisposedException)
        {
            // 流已释放，返回 false
            return false;
        }
    }

    /// <summary>
    /// Returns the memory buffer. For the block-based reader, individual returns are not tracked — blocks are returned
    /// in bulk via the BlockAllocator when the LogBuffer is evicted or the reader is disposed.
    /// </summary>
    public void ReturnMemory (ReadOnlyMemory<char> memory)
    {
        // Bulk return via BlockAllocator.DetachBlocks() or Dispose().
        // Individual per-line return is not needed with block-based allocation.
    }

    #endregion

    #region Private Methods

    private int GuessNewLineSequenceLength (StreamReader reader)
    {
        var currentPos = Position;
        var originalStreamPos = reader.BaseStream.Position; // 记录原始流位置
        try
        {
            var line = reader.ReadLine();
            if (line == null) return 0;

            // 仅累加行内容的字节数（不含换行符）
            var lineBytes = Encoding.GetByteCount(line);
            Position += lineBytes;

            int newLineByteCount = 0;
            var firstChar = reader.Read();
            if (firstChar == CHAR_CR)
            {
                var secondChar = reader.Read();
                if (secondChar == CHAR_LF)
                {
                    Span<char> crlf = ['\r', '\n'];
                    newLineByteCount = Encoding.GetByteCount(crlf);
                }
                else
                {
                    // 仅 \r，回退第二个字符的读取位置
                    if (secondChar != -1) reader.BaseStream.Position--;
                    Span<char> cr = ['\r'];
                    newLineByteCount = Encoding.GetByteCount(cr);
                }
            }
            else if (firstChar == CHAR_LF || firstChar != -1)
            {
                // 仅 \n 或其他单个换行符
                Span<char> single = [(char)firstChar];
                newLineByteCount = Encoding.GetByteCount(single);
            }

            return newLineByteCount;
        }
        finally
        {
            // 还原流位置和 Position，避免影响后续读取
            Position = currentPos;
            reader.BaseStream.Position = originalStreamPos;
        }
    }

    protected override void Dispose (bool disposing)
    {
        if (disposing)
        {
            BlockAllocator?.Dispose();
            BlockAllocator = null;
        }

        base.Dispose(disposing);
    }

    #endregion
}