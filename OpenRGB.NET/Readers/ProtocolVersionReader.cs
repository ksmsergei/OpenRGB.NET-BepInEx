using ksm.OpenRGB.Utils;

namespace ksm.OpenRGB;

internal readonly struct ProtocolVersionReader : ISpanReader<ProtocolVersion>
{
    public ProtocolVersion ReadFrom(ref SpanReader reader, ProtocolVersion? p = default, int? i = default, int? outerCount = default)
        => ProtocolVersion.FromNumber(reader.Read<uint>());
}