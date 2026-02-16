using OpenRGB_BepInEx.OpenRGB.NET.Utils;

namespace OpenRGB_BepInEx.OpenRGB.NET.Writables;

internal readonly struct EmptyArg : ISpanWritable
{
    public int Length => 0;
    public void WriteTo(ref SpanWriter writer) { }
}