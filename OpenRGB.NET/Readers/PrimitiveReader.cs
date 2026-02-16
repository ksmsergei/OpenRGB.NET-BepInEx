using OpenRGB_BepInEx.OpenRGB.NET.Models;
using OpenRGB_BepInEx.OpenRGB.NET.Utils;

namespace OpenRGB_BepInEx.OpenRGB.NET.Readers;

internal readonly struct PrimitiveReader<T> : ISpanReader<T> where T : unmanaged
{
    public T ReadFrom(ref SpanReader reader, ProtocolVersion? protocolVersion = default, int? index = default, int? outerCount = default) =>
        reader.Read<T>();
}