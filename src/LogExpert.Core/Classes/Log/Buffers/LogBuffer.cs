using System;
using System.Collections.Generic;

using ColumnizerLib;

using LogExpert.Core.Classes.Log.Buffers;

public class LogBuffer
{
    private List<RcCharBlock>? _rcCharBlocks;
    private readonly List<LogLine> _lines = new();
    private readonly object _contentLock = new();

    public bool IsDisposed { get; private set; }
    public bool IsPinned { get; private set; }
    public long DisposeCount { get; private set; }

    public ILogFileInfo FileInfo { get; set; }
    public int StartLine { get; set; }
    public long StartPos { get; set; }
    public long Size { get; set; }
    public int LineCount => _lines.Count;
    public int DroppedLinesCount { get; set; }
    public int PrevBuffersDroppedLinesSum { get; set; }

    private readonly List<long> _lineFilePositions = new List<long>();

    public LogBuffer(ILogFileInfo fileInfo, int maxLines)
    {
        FileInfo = fileInfo;
    }

    // 唯一的内存挂载方法
    public void AttachRcBlocks(List<RcCharBlock> blocks)
    {
        lock (_contentLock)
        {
            ReleaseRcBlocks();
            _rcCharBlocks = blocks;
        }
    }

    private void ReleaseRcBlocks()
    {
        if (_rcCharBlocks != null)
        {
            foreach (var block in _rcCharBlocks)
                block?.Release();

            _rcCharBlocks = null;
        }
    }

    public void EvictContent()
    {
        lock (_contentLock)
        {
            if (IsPinned) return;

            _lines.Clear();
            _lineFilePositions.Clear();
            ReleaseRcBlocks();
            IsDisposed = true;
            DisposeCount++;
        }
    }

    public void DisposeContent() => EvictContent();

    public void ClearLines()
    {
        lock (_contentLock)
        {
            _lines.Clear();
            _lineFilePositions.Clear();
            ReleaseRcBlocks();
        }
    }

    public void Reinitialise(ILogFileInfo fileInfo, int maxLines)
    {
        lock (_contentLock)
        {
            ClearLines();
            FileInfo = fileInfo;
            StartLine = 0;
            StartPos = 0;
            Size = 0;
            DroppedLinesCount = 0;
            PrevBuffersDroppedLinesSum = 0;
            IsDisposed = false;
            DisposeCount = 0;
        }
    }

    public void Pin() => IsPinned = true;
    public void Unpin() => IsPinned = false;

    public void AddLine(LogLine line, long filePos)
    {
        lock (_contentLock)
        {
            _lines.Add(line);
            _lineFilePositions.Add(filePos);
        }
    }

    public LogLine? GetLineMemoryOfBlock(int index)
    {
        lock (_contentLock)
        {
            if (index < 0 || index >= _lines.Count)
                return null;

            return _lines[index];
        }
    }

    public long GetFilePosForLineOfBlock(int localLineIndex)
    {
        lock (_contentLock)
        {
            if (localLineIndex < 0 || localLineIndex >= _lineFilePositions.Count)
                return -1;

            return _lineFilePositions[localLineIndex];
        }
    }

    public void AcquireContentLock(ref bool lockTaken)
    {
        System.Threading.Monitor.Enter(_contentLock, ref lockTaken);
    }

    public void ReleaseContentLock()
    {
        System.Threading.Monitor.Exit(_contentLock);
    }
}