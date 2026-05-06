using System.Buffers;

namespace LogExpert.Core.Classes.Log.Buffers;

public sealed class CharBlockAllocator : IDisposable
{
    private const int DEFAULT_BLOCK_SIZE = 32768; // 64 KB (32K chars × 2 bytes), stays under 85 KB LOH threshold

    private readonly int _blockSize;
    private List<RcCharBlock> _blocks = new();
    private List<RcCharBlock> _oversizedBlocks = new();
    private RcCharBlock _currentBlock = null!;
    private int _currentOffset;
    private bool _disposed;

    public CharBlockAllocator (int blockSize = DEFAULT_BLOCK_SIZE)
    {
        _blockSize = blockSize;
        _currentBlock = RcCharBlock.Rent(_blockSize);
        _blocks.Add(_currentBlock);
        _currentOffset = 0;
    }

    // 供测试使用
    public int BlockCount => _blocks.Count;
    public int OversizedBlockCount => _oversizedBlocks.Count;

    public ReadOnlyMemory<char> Allocate(ReadOnlySpan<char> content)
    {
        int length = content.Length;

        if (length >= LogMemoryPool.LargeLineThreshold)
        {
            var large = RcCharBlock.Rent(length);
            content.CopyTo(large.Buffer);
            _oversizedBlocks.Add(large);
            return large.Slice(0, length);
        }

        // 空间不足则新建块
        if (_currentOffset + length > _currentBlock.Capacity)
        {
            _currentBlock = RcCharBlock.Rent(_blockSize);
            _blocks.Add(_currentBlock);
            _currentOffset = 0;
        }

        // 拷贝内容+切片
        content.CopyTo(_currentBlock.Buffer.AsSpan(_currentOffset));
        var memory = _currentBlock.Slice(_currentOffset, length);
        _currentOffset += length;
        return memory;
    }

    //
    public Memory<char> Rent(int length)
    {
        if (length == 0)
            return Memory<char>.Empty;

        if (length >= LogMemoryPool.LargeLineThreshold)
        {
            var largeBlock = RcCharBlock.Rent(length);
            _oversizedBlocks.Add(largeBlock);
            return largeBlock.Buffer.AsMemory(0, length);
        }

        if (_currentOffset + length > _currentBlock.Capacity)
        {
            _currentBlock = RcCharBlock.Rent(_blockSize);
            _blocks.Add(_currentBlock);
            _currentOffset = 0;
        }

        var mem = _currentBlock.Buffer.AsMemory(_currentOffset, length);
        _currentOffset += length;
        return mem;
    }

    public bool TryRent (int length, out Memory<char> memory)
    {
        try
        {
            memory = Rent(length);
            return true;
        }
        catch (OutOfMemoryException)
        {
            memory = default;
            return false;
        }
    }

    /// <summary>
    /// 剥离所有RC内存块，移交LogBuffer管理
    /// </summary>
    public List<RcCharBlock> DetachRcBlocks()
    {
        var list = _blocks;
        _currentBlock = RcCharBlock.Rent(_blockSize);
        _blocks = new List<RcCharBlock> { _currentBlock };
        _currentOffset = 0;
        return list;
    }


    public void ReturnAll()
    {
        foreach (var block in _blocks) block.Dispose();
        foreach (var block in _oversizedBlocks) block.Dispose();
        _blocks.Clear();
        _oversizedBlocks.Clear();
    }

    public void Dispose ()
    {
        if (!_disposed)
        {
            ReturnAll();
            _disposed = true;
        }
    }
}