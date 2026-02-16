using OpenRGB_BepInEx.OpenRGB.NET.Utils;

namespace OpenRGB_BepInEx.OpenRGB.NET.Writables;

internal readonly record struct StringArg(string Value) : ISpanWritable
{
    public int Length => Value.Length + 1;
    public void WriteTo(ref SpanWriter writer) => writer.Write(Value);
}