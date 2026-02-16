using OpenRGB_BepInEx.OpenRGB.NET.Models;
using OpenRGB_BepInEx.OpenRGB.NET.Utils;

namespace OpenRGB_BepInEx.OpenRGB.NET.Readers;

internal readonly struct ProtocolVersionReader : ISpanReader<ProtocolVersion>
{
    public ProtocolVersion ReadFrom(ref SpanReader reader, ProtocolVersion? p = default, int? i = default, int? outerCount = default)
        => ProtocolVersion.FromNumber(reader.Read<uint>());
}