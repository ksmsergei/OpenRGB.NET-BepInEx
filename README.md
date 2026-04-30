# OpenRGB.NET for BepInEx

This is a fork of OpenRGB C# library. It provides easy way to manipulate with OpenRGB within BepInEx plugins.

You can check the original repo here:

https://github.com/diogotr7/OpenRGB.NET

## Quick Start

```csharp
using ksm.OpenRGB;

// Connect to OpenRGB (auto-connects by default)
using var client = new OpenRgbClient(name: "MyPlugin");

// Discover all RGB devices
Device[] devices = client.GetAllControllerData();
foreach (var device in devices)
    Logger.LogInfo($"[{device.Index}] {device.Type}: {device.Name}");

// Set all LEDs on device 0 to red
var deviceId = 0;
client.SetCustomMode(deviceId);

var device0 = client.GetControllerData(deviceId);
var colors = new Color[device0.Leds.Length];
for (int i = 0; i < colors.Length; i++)
    colors[i] = new Color(255, 0, 0);

client.UpdateLeds(deviceId, colors);
```

## Documentation

Full documentation is available in the [`docs/`](docs/) folder:

| Document | Description |
|---|---|
| [Getting Started](docs/getting-started.md) | Setup, connection, and first steps |
| [API Reference](docs/api-reference.md) | All classes, methods, and types |
| [Examples](docs/examples.md) | Common usage patterns |

## Original Repository

https://github.com/diogotr7/OpenRGB.NET