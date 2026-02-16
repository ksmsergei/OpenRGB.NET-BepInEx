using OpenRGB_BepInEx.OpenRGB.NET.Models;
using OpenRGB_BepInEx.OpenRGB.NET.Utils;

namespace OpenRGB_BepInEx.OpenRGB.NET;

internal interface ISpanReader<out T>
{
    T ReadFrom(ref SpanReader reader, ProtocolVersion? protocolVersion = default, int? index = default, int? outerCount = default);
}