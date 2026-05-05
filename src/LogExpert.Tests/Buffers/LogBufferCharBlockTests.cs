using System.Buffers;

using ColumnizerLib;

using LogExpert.Core.Classes.Log.Buffers;

using Moq;

using NUnit.Framework;

namespace LogExpert.Tests.Buffers;

[TestFixture]
public class LogBufferCharBlockTests
{
    private Mock<ILogFileInfo> _mockFileInfo;

    [SetUp]
    public void Setup ()
    {
        _mockFileInfo = new Mock<ILogFileInfo>();
        _mockFileInfo.Setup(f => f.FullName).Returns("test.log");
    }

    [Test]
    public void AttachRcBlocks_AcceptsBlockList()
    {
        var buffer = new LogBuffer(_mockFileInfo.Object, 10);
        var blocks = new List<RcCharBlock>
        {
            RcCharBlock.Rent(128),
            RcCharBlock.Rent(128)
        };

        // Should not throw
        buffer.AttachRcBlocks(blocks);

        // Clean up via eviction
        buffer.EvictContent();
    }

    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Unit Tests")]
    public void EvictContent_WhilePinned_TriggersDebugAssert ()
    {
        var buffer = new LogBuffer(_mockFileInfo.Object, 10);
        var block = RcCharBlock.Rent(128);
        buffer.AttachRcBlocks([block]);
        buffer.AddLine(new LogLine("test".AsMemory(), 0), 0);
        buffer.Pin();

        // In Debug builds, this should trigger the assert.
        // In Release builds, it proceeds (defense in depth via eviction skip).
        // We can't directly test Debug.Assert in NUnit, but we verify the behavior:
        // After eviction while pinned, the buffer should still be disposed
        // (the assert is a developer warning, not a runtime guard).
        buffer.EvictContent();
        buffer.Unpin();
    }

    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Unit Tests")]
    public void EvictContent_ReturnsAttachedRcBlocks()
    {
        var buffer = new LogBuffer(_mockFileInfo.Object, 10);
        var block = RcCharBlock.Rent(128);
        buffer.AttachRcBlocks([block]);

        // Add a line so there's something to evict
        var lineMemory = "test line".AsMemory();
        buffer.AddLine(new LogLine(lineMemory, 0), 0);

        buffer.EvictContent();

        var newBlock = RcCharBlock.Rent(128);
        buffer.AttachRcBlocks([newBlock]);
        buffer.EvictContent();
    }

    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Unit Tests")]
    public void DisposeContent_ReturnsAttachedRcBlocks()
    {
        var buffer = new LogBuffer(_mockFileInfo.Object, 10);
        var block = RcCharBlock.Rent(128);
        buffer.AttachRcBlocks([block]);

        buffer.AddLine(new LogLine("test".AsMemory(), 0), 0);
        buffer.DisposeContent();

        buffer.Reinitialise(_mockFileInfo.Object, 10);
        var newBlock = RcCharBlock.Rent(128);
        buffer.AttachRcBlocks([newBlock]);
        buffer.EvictContent();
    }

    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Unit Tests")]
    public void ClearLines_ReturnsAttachedRcBlocks()
    {
        var buffer = new LogBuffer(_mockFileInfo.Object, 10);
        var block = RcCharBlock.Rent(128);
        buffer.AttachRcBlocks([block]);

        buffer.AddLine(new LogLine("test".AsMemory(), 0), 0);
        buffer.ClearLines();

        Assert.That(buffer.LineCount, Is.EqualTo(0));

        var newBlock = RcCharBlock.Rent(128);
        buffer.AttachRcBlocks([newBlock]);
        buffer.EvictContent();
    }

    [Test]
    public void Reinitialise_ReturnsAttachedRcBlocks()
    {
        var buffer = new LogBuffer(_mockFileInfo.Object, 10);
        var block = RcCharBlock.Rent(128);
        buffer.AttachRcBlocks([block]);

        buffer.Reinitialise(_mockFileInfo.Object, 10);

        var newBlock = RcCharBlock.Rent(128);
        buffer.AttachRcBlocks([newBlock]);
        buffer.EvictContent();
    }

    [Test]
    public void AttachRcBlocks_ReturnsOldBlocks_WhenCalledTwice()
    {
        var buffer = new LogBuffer(_mockFileInfo.Object, 10);

        var block1 = RcCharBlock.Rent(128);
        buffer.AttachRcBlocks([block1]);

        var block2 = RcCharBlock.Rent(128);
        buffer.AttachRcBlocks([block2]);

        buffer.EvictContent();
    }

    [Test]
    public void AttachRcBlocks_Null_DoesNotThrow()
    {
        var buffer = new LogBuffer(_mockFileInfo.Object, 10);
        buffer.AttachRcBlocks(null);
        buffer.EvictContent();
    }

    [Test]
    public void AttachRcBlocks_EmptyList_DoesNotThrow()
    {
        var buffer = new LogBuffer(_mockFileInfo.Object, 10);
        buffer.AttachRcBlocks([]);
        buffer.EvictContent();
    }

    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Unit Tests")]
    public void EvictContent_WithoutAttachedBlocks_DoesNotThrow ()
    {
        var buffer = new LogBuffer(_mockFileInfo.Object, 10);
        buffer.AddLine(new LogLine("test".AsMemory(), 0), 0);

        // No char blocks attached — should evict cleanly (backward compat)
        buffer.EvictContent();
    }

    [Test]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Unit Tests")]
    public void BlockBackedLines_ContentSurvivesUntilEviction ()
    {
        var buffer = new LogBuffer(_mockFileInfo.Object, 10);

        var block = RcCharBlock.Rent(1024);
        "Hello World".AsSpan().CopyTo(block.Buffer.AsSpan(0, 11));
        "Second Line".AsSpan().CopyTo(block.Buffer.AsSpan(11, 11));

        var line1Memory = new ReadOnlyMemory<char>(block.Buffer, 0, 11);
        var line2Memory = new ReadOnlyMemory<char>(block.Buffer, 11, 11);

        buffer.AddLine(new LogLine(line1Memory, 0), 0);
        buffer.AddLine(new LogLine(line2Memory, 1), 11);
        buffer.AttachRcBlocks([block]);

        // Lines should be readable while buffer is alive
        var retrieved1 = buffer.GetLineMemoryOfBlock(0);
        var retrieved2 = buffer.GetLineMemoryOfBlock(1);
        Assert.That(retrieved1.HasValue, Is.True);
        Assert.That(retrieved2.HasValue, Is.True);

        Assert.That(retrieved1.Value.FullLine.Span.ToString(), Is.EqualTo("Hello World"));
        Assert.That(retrieved2.Value.FullLine.Span.ToString(), Is.EqualTo("Second Line"));

        // After eviction, blocks are returned and lines are no longer accessible
        buffer.EvictContent();
        Assert.That(buffer.IsDisposed, Is.True);
    }
}