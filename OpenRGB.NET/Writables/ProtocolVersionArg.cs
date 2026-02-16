using OpenRGB_BepInEx.OpenRGB.NET.Models;
using OpenRGB_BepInEx.OpenRGB.NET.Utils;

namespace OpenRGB_BepInEx.OpenRGB.NET.Writables;

internal readonly record struct ProtocolVersionArg(ProtocolVersion Version) : ISpanWritable
{
    public int Length => sizeof(uint);
    public void WriteTo(ref SpanWriter writer) => writer.Write(Version.Number);
}