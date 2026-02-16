using OpenRGB_BepInEx.OpenRGB.NET.Utils;

namespace OpenRGB_BepInEx.OpenRGB.NET;

internal interface ISpanWritable
{
    int Length { get; }
    void WriteTo(ref SpanWriter writer);
}