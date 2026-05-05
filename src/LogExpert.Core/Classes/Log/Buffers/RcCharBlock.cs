using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogExpert.Core.Classes.Log.Buffers;
/// <summary>
/// 引用计数池化内存块，计数归零自动归还，彻底解决UseAfterReturn
/// </summary>
public sealed class RcCharBlock : IDisposable
{
    public char[] Buffer { get; }
    public int Capacity { get; }

    private int _refCount;
    private bool _disposed;

    private RcCharBlock (char[] buffer, int capacity)
    {
        Buffer = buffer;
        Capacity = capacity;
    }

    public static RcCharBlock Rent (int minSize)
    {
        var buf = LogMemoryPool.CharPool.Rent(minSize);
        return new RcCharBlock(buf, buf.Length);
    }

    /// <summary>
    /// 切片并自动增加引用计数
    /// </summary>
    public ReadOnlyMemory<char> Slice (int start, int length)
    {
        if (start + length > Capacity)
            throw new ArgumentOutOfRangeException(nameof(length));

        Interlocked.Increment(ref _refCount);
        return Buffer.AsMemory(start, length);
    }

    /// <summary>
    /// 释放引用，无引用则回收内存
    /// </summary>
    public void Release ()
    {
        if (_disposed) return;
        if (Interlocked.Decrement(ref _refCount) <= 0) Dispose();
    }

    public void Dispose ()
    {
        if (_disposed) return;
        _disposed = true;
        LogMemoryPool.CharPool.Return(Buffer);
    }
}