using ksm.OpenRGB.Utils;

namespace ksm.OpenRGB;

internal readonly record struct ModeOperationArg(uint ModeId, ModeArg Mode) : ISpanWritable
{
    public int Length => sizeof(uint) * 2 + Mode.Length;
    public void WriteTo(ref SpanWriter writer)
    {
        writer.Write(Length);
        writer.Write(ModeId);
        Mode.WriteTo(ref writer);
    }
}
