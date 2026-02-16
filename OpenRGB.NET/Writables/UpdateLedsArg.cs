using System.Runtime.CompilerServices;
using ksm.OpenRGB.Utils;

namespace ksm.OpenRGB;

internal readonly record struct UpdateLedsArg(ushort ColorCount) : ISpanWritable
{
    public int Length => sizeof(uint) +
                         sizeof(ushort);
    
    public void WriteTo(ref SpanWriter writer)
    {
        unsafe { writer.Write((uint)(Length + sizeof(Color) * ColorCount)); }
        writer.Write(ColorCount);
    }
}