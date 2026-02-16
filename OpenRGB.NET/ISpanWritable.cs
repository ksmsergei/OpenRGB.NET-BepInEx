using ksm.OpenRGB.Utils;

namespace ksm.OpenRGB;

internal interface ISpanWritable
{
    int Length { get; }
    void WriteTo(ref SpanWriter writer);
}