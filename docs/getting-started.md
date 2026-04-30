# Getting Started

## Requirements

- **OpenRGB** — must be running with the SDK server enabled.  
  In OpenRGB: *Settings → SDK Server → Start Server* (default port `6742`).
- **BepInEx** — installed in your game directory.
- A C# BepInEx plugin project targeting .NET Standard 2.1 or later.

## Installation

Add the compiled `OpenRGB-BepInEx.dll` to your project references or copy it into your BepInEx `plugins` folder alongside your own plugin DLL.

The namespace for all types is `ksm.OpenRGB`.

```csharp
using ksm.OpenRGB;
```

## Connecting to OpenRGB

Create an `OpenRgbClient` instance. By default it connects immediately on construction:

```csharp
// Connect with default settings (localhost:6742)
using var client = new OpenRgbClient();

// Or with custom parameters
using var client = new OpenRgbClient(
    ip: "127.0.0.1",
    port: 6742,
    name: "MyPlugin",      // name shown in OpenRGB
    autoConnect: true,
    timeoutMs: 1000
);
```

If you set `autoConnect: false`, call `Connect()` manually when ready:

```csharp
var client = new OpenRgbClient(autoConnect: false);
// ... later ...
client.Connect();
```

Always dispose the client when finished so the socket is cleaned up properly:

```csharp
client.Dispose();
// or use `using` statement / declaration
```

## Checking Connection Status

```csharp
if (client.Connected)
{
    // safe to call SDK methods
}
```

## Discovering Devices

```csharp
// Get all devices in one call
Device[] devices = client.GetAllControllerData();

foreach (var device in devices)
{
    Console.WriteLine($"[{device.Index}] {device.Type}: {device.Name}");
}
```

> **Important:** Device indices may change if OpenRGB rescans devices (e.g. after a USB reconnect). Always re-fetch device data after a `DeviceListUpdated` event rather than caching indices long-term.

## Reacting to Device List Changes

Subscribe to the `DeviceListUpdated` event to be notified when OpenRGB detects a change:

```csharp
client.DeviceListUpdated += (sender, args) =>
{
    // Re-fetch all device data
    var updated = client.GetAllControllerData();
};
```

> **Note:** The server may fire this event several times in rapid succession during a single rescan. Consider debouncing your handler to avoid re-fetching device data multiple times per rescan cycle.

## Setting Colors

There are two approaches depending on what you want to do:

### Direct LED color writes (`UpdateLeds` / `UpdateZoneLeds` / `UpdateSingleLed`)

Call `SetCustomMode` first to put the device into the built-in "Custom" mode, then push color data:

```csharp
int deviceId = 0;
client.SetCustomMode(deviceId);

var device = client.GetControllerData(deviceId);
var colors = new Color[device.Leds.Length];

// Fill all LEDs with red
for (int i = 0; i < colors.Length; i++)
    colors[i] = new Color(255, 0, 0);

client.UpdateLeds(deviceId, colors);
```

### Activating a built-in hardware mode (`UpdateMode`)

`SetCustomMode` is **not** required here. Fetch the device, find the mode by name or index, and call `UpdateMode` directly:

```csharp
var device = client.GetControllerData(0);
int modeIndex = Array.FindIndex(device.Modes, m => m.Name == "Breathing");
if (modeIndex >= 0)
    client.UpdateMode(device.Index, modeIndex, speed: device.Modes[modeIndex].SpeedMin);
```

## Next Steps

- [API Reference](api-reference.md) — full method and type reference
- [Examples](examples.md) — practical patterns for common scenarios
