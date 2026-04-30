# Examples

All examples assume the following using directive:

```csharp
using ksm.OpenRGB;
```

---

## Basic BepInEx Plugin Setup

```csharp
using BepInEx;
using ksm.OpenRGB;

[BepInPlugin("com.yourname.myplugin", "My RGB Plugin", "1.0.0")]
public class MyPlugin : BaseUnityPlugin
{
    private OpenRgbClient? _rgbClient;

    private void Awake()
    {
        try
        {
            _rgbClient = new OpenRgbClient(name: "MyPlugin");
            Logger.LogInfo("Connected to OpenRGB.");
        }
        catch (Exception ex)
        {
            Logger.LogWarning($"Could not connect to OpenRGB: {ex.Message}");
        }
    }

    private void OnDestroy()
    {
        _rgbClient?.Dispose();
    }
}
```

---

## List All Devices

```csharp
Device[] devices = client.GetAllControllerData();

foreach (var device in devices)
{
    Logger.LogInfo($"Device [{device.Index}]: {device.Type} — {device.Name}");
    Logger.LogInfo($"  Vendor: {device.Vendor ?? "N/A"}");
    Logger.LogInfo($"  LEDs: {device.Leds.Length}, Zones: {device.Zones.Length}");
    Logger.LogInfo($"  Active mode: {device.ActiveMode.Name}");
}
```

---

## Set All LEDs to a Solid Color

```csharp
void SetAllToColor(OpenRgbClient client, Color color)
{
    var devices = client.GetAllControllerData();
    foreach (var device in devices)
    {
        client.SetCustomMode(device.Index);

        var colors = new Color[device.Leds.Length];
        Array.Fill(colors, color);
        client.UpdateLeds(device.Index, colors);
    }
}

// Usage
SetAllToColor(client, new Color(255, 0, 0));   // red
SetAllToColor(client, new Color(0, 0, 0));     // off
```

---

## Update a Single LED

```csharp
int deviceId = 0;
int ledId    = 5;

client.SetCustomMode(deviceId);
client.UpdateSingleLed(deviceId, ledId, new Color(0, 255, 128));
```

---

## Update LEDs in a Specific Zone

```csharp
var device = client.GetControllerData(0);
var zone   = device.Zones[0];  // first zone

client.SetCustomMode(device.Index);

var colors = new Color[zone.LedCount];
for (int i = 0; i < colors.Length; i++)
    colors[i] = new Color((byte)(i * 10 % 256), 0, 255);  // gradient

client.UpdateZoneLeds(device.Index, zone.Index, colors);
```

---

## Rainbow Effect Across All Devices

```csharp
void ApplyRainbow(OpenRgbClient client, float offset)
{
    var devices = client.GetAllControllerData();
    foreach (var device in devices)
    {
        client.SetCustomMode(device.Index);

        var colors = new Color[device.Leds.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            float hue = (offset + (float)i / colors.Length) % 1f;
            colors[i] = HsvToRgb(hue, 1f, 1f);
        }
        client.UpdateLeds(device.Index, colors);
    }
}

Color HsvToRgb(float h, float s, float v)
{
    int hi = (int)(h * 6) % 6;
    float f = h * 6 - (int)(h * 6);
    byte p = (byte)(v * (1 - s) * 255);
    byte q = (byte)(v * (1 - f * s) * 255);
    byte t = (byte)(v * (1 - (1 - f) * s) * 255);
    byte vb = (byte)(v * 255);
    return hi switch
    {
        0 => new Color(vb, t, p),
        1 => new Color(q, vb, p),
        2 => new Color(p, vb, t),
        3 => new Color(p, q, vb),
        4 => new Color(t, p, vb),
        _ => new Color(vb, p, q)
    };
}
```

---

## Set a Built-in Mode (e.g. "Breathing")

```csharp
var device = client.GetControllerData(0);

// Find a mode by name
int modeIndex = Array.FindIndex(device.Modes, m => m.Name == "Breathing");
if (modeIndex < 0)
{
    Logger.LogWarning("Breathing mode not found.");
    return;
}

var mode = device.Modes[modeIndex];

client.UpdateMode(
    deviceId:  device.Index,
    modeId:    modeIndex,
    speed:     mode.SupportsSpeed    ? mode.SpeedMin : (uint?)null,
    direction: mode.SupportsDirection ? Direction.Left : (Direction?)null,
    colors:    mode.ColorMode == ColorMode.ModeSpecific
                   ? new[] { new Color(0, 128, 255) }
                   : null
);
```

---

## Handling Device List Changes

```csharp
Device[] _devices = client.GetAllControllerData();

client.DeviceListUpdated += (_, _) =>
{
    // Device indices may have shifted — always re-fetch
    _devices = client.GetAllControllerData();
    Logger.LogInfo($"Device list refreshed: {_devices.Length} devices.");
};
```

> **Note:** Device IDs (indices) are positional and can change when OpenRGB rescans hardware. Never persist device IDs across a `DeviceListUpdated` event.

---

## Working with Profiles

```csharp
// Requires OpenRGB protocol version >= 2

string[] profiles = client.GetProfiles();
Logger.LogInfo("Saved profiles: " + string.Join(", ", profiles));

// Load a profile
client.LoadProfile("MyProfile");

// Save current state as a new profile
client.SaveProfile("MyNewProfile");

// Delete a profile
client.DeleteProfile("OldProfile");
```

---

## Turning Off All LEDs on Dispose

```csharp
void TurnOffAll(OpenRgbClient client)
{
    var devices = client.GetAllControllerData();
    var black = new Color(0, 0, 0);

    foreach (var device in devices)
    {
        client.SetCustomMode(device.Index);
        client.UpdateLeds(device.Index, new Color[device.Leds.Length]);  // all black
    }
}
```

---

## Error Handling

```csharp
try
{
    using var client = new OpenRgbClient(timeoutMs: 500);
    // use client ...
}
catch (System.Net.Sockets.SocketException)
{
    Logger.LogWarning("Could not connect to OpenRGB. Is the SDK server running?");
}
catch (NotSupportedException ex)
{
    Logger.LogWarning($"Feature not supported by server: {ex.Message}");
}
```
