using ksm.OpenRGB.Utils;

namespace ksm.OpenRGB;

internal interface ISpanReader<out T>
{
    T ReadFrom(ref SpanReader reader, ProtocolVersion? protocolVersion = default, int? index = default, int? outerCount = default);
}