using ksm.OpenRGB.Utils;

namespace ksm.OpenRGB;

internal readonly struct PrimitiveReader<T> : ISpanReader<T> where T : unmanaged
{
    public T ReadFrom(ref SpanReader reader, ProtocolVersion? protocolVersion = default, int? index = default, int? outerCount = default) =>
        reader.Read<T>();
}