using System;

namespace OpenRGB_BepInEx.OpenRGB.NET.Utils;

#if DEBUG


[AttributeUsage(AttributeTargets.Struct)]
internal class NonCopyableAttribute : Attribute { }

#endif