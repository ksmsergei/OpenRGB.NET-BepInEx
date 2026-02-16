using ksm.OpenRGB.Utils;

namespace ksm.OpenRGB;

internal readonly struct EmptyArg : ISpanWritable
{
    public int Length => 0;
    public void WriteTo(ref SpanWriter writer) { }
}