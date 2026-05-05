using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogExpert.Core.Classes.Log.Buffers;
internal static class LogMemoryPool
{
    public static readonly ArrayPool<char> CharPool = ArrayPool<char>.Create(
        maxArrayLength: 65536,
        maxArraysPerBucket: 30);

    public const int LargeLineThreshold = 8192;
}
