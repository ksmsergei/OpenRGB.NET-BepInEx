# API Reference

Namespace: `ksm.OpenRGB`

---

## `OpenRgbClient`

The main entry point. Implements `IOpenRgbClient` and `IDisposable`.

### Constructor

```csharp
public OpenRgbClient(
    string ip = "127.0.0.1",
    int port = 6742,
    string name = "OpenRGB.NET",
    bool autoConnect = true,
    int timeoutMs = 1000,
    uint protocolVersionNumber = 4)
```

| Parameter | Default | Description |
|---|---|---|
| `ip` | `"127.0.0.1"` | IP address of the OpenRGB server |
| `port` | `6742` | Port of the OpenRGB SDK server |
| `name` | `"OpenRGB.NET"` | Client name shown in OpenRGB UI |
| `autoConnect` | `true` | Connect immediately on construction |
| `timeoutMs` | `1000` | Socket timeout in milliseconds |
| `protocolVersionNumber` | `4` | Maximum protocol version to negotiate (0–4) |

Throws `ArgumentException` if `protocolVersionNumber` exceeds the maximum supported value (4).

### Properties

| Property | Type | Description |
|---|---|---|
| `Connected` | `bool` | Whether the socket is currently connected |
| `MaxSupportedProtocolVersion` | `ProtocolVersion` | Highest protocol version this build supports |
| `ClientProtocolVersion` | `ProtocolVersion` | Protocol version requested by this client |
| `CommonProtocolVersion` | `ProtocolVersion` | Negotiated version actually in use (set after `Connect()`) |

### Events

| Event | Description |
|---|---|
| `DeviceListUpdated` | Fired when the server's device list changes. Re-fetch all device data when this fires. The server may fire this event multiple times per rescan; consider debouncing your handler. |

### Methods

#### `Connect()`
```csharp
void Connect()
```
Manually connects to the server. No-op if already connected. Required when `autoConnect` is `false`.

---

#### `GetControllerCount()`
```csharp
int GetControllerCount()
```
Returns the total number of RGB controllers detected by the server.

---

#### `GetControllerData(int deviceId)`
```csharp
Device GetControllerData(int deviceId)
```
Returns full device information for the given device index.

---

#### `GetAllControllerData()`
```csharp
Device[] GetAllControllerData()
```
Convenience method — calls `GetControllerCount()` and `GetControllerData()` for each index.

---

#### `SetCustomMode(int deviceId)`
```csharp
void SetCustomMode(int deviceId)
```
Switches the device to the built-in "Custom" lighting mode. Call this before writing raw LED colors with `UpdateLeds`, `UpdateZoneLeds`, or `UpdateSingleLed`. It is **not** required before `UpdateMode`, which activates hardware modes directly.

---

#### `UpdateLeds(int deviceId, ReadOnlySpan<Color> colors)`
```csharp
void UpdateLeds(int deviceId, ReadOnlySpan<Color> colors)
```
Updates all LEDs on a device at once. The `colors` span must have exactly `device.Leds.Length` elements.

---

#### `UpdateZoneLeds(int deviceId, int zoneId, ReadOnlySpan<Color> colors)`
```csharp
void UpdateZoneLeds(int deviceId, int zoneId, ReadOnlySpan<Color> colors)
```
Updates all LEDs in a specific zone. The `colors` span must have `zone.LedCount` elements.

---

#### `UpdateSingleLed(int deviceId, int ledId, Color color)`
```csharp
void UpdateSingleLed(int deviceId, int ledId, Color color)
```
Updates a single LED by its index on the device.

---

#### `UpdateMode(int deviceId, int modeId, ...)`
```csharp
void UpdateMode(
    int deviceId,
    int modeId,
    uint? speed = null,
    Direction? direction = null,
    Color[]? colors = null)
```
Sets a specific mode (by index from `device.Modes`) with optional overrides. Parameters not provided retain their current server-side values.

Throws `InvalidOperationException` if the mode does not support the specified optional parameter (e.g. setting speed on a mode without `ModeFlags.HasSpeed`).

---

#### `SaveMode(int deviceId, int modeId)`
```csharp
void SaveMode(int deviceId, int modeId)
```
Persists the current mode configuration on the device.

---

#### `ResizeZone(int deviceId, int zoneId, int size)`
```csharp
void ResizeZone(int deviceId, int zoneId, int size)
```
Resizes a resizable zone (e.g. an LED strip) to the given LED count.

---

#### `GetProfiles()`
```csharp
string[] GetProfiles()
```
Returns the names of all saved profiles.  
Requires protocol version ≥ 2 (`CommonProtocolVersion.SupportsProfileControls`). Throws `NotSupportedException` otherwise.

---

#### `LoadProfile(string profile)`
```csharp
void LoadProfile(string profile)
```
Loads a saved profile by name.

---

#### `SaveProfile(string profile)`
```csharp
void SaveProfile(string profile)
```
Saves the current state as a profile with the given name.

---

#### `DeleteProfile(string profile)`
```csharp
void DeleteProfile(string profile)
```
Deletes a profile by name.

---

#### `GetPlugins()`
```csharp
Plugin[] GetPlugins()
```
Returns server-side OpenRGB plugins.  
Requires protocol version ≥ 4 (`CommonProtocolVersion.SupportsSegmentsAndPlugins`).

---

#### `PluginSpecific(int pluginId, int pluginPacketType, ReadOnlySpan<byte> data)`
```csharp
void PluginSpecific(int pluginId, int pluginPacketType, ReadOnlySpan<byte> data)
```
Sends a raw data packet to an OpenRGB server plugin.  
Requires protocol version ≥ 4.

---

#### `Dispose()`
```csharp
void Dispose()
```
Closes the TCP connection and releases resources.

---

## Models

### `Color`

```csharp
public readonly record struct Color(byte R = 0, byte G = 0, byte B = 0)
```

Represents an RGB color. Stored as 4 bytes (R, G, B + 1 byte padding) for alignment with the OpenRGB wire format.

```csharp
var red   = new Color(255, 0, 0);
var green = new Color(0, 255, 0);
var blue  = new Color(0, 0, 255);
var black = new Color();          // R=0, G=0, B=0
```

---

### `Device`

Represents an RGB controller. Read-only; obtained from `GetControllerData()` / `GetAllControllerData()`.

| Property | Type | Description |
|---|---|---|
| `Index` | `int` | Zero-based device index |
| `Type` | `DeviceType` | Category of device |
| `Name` | `string` | Device name |
| `Vendor` | `string?` | Vendor name (null on protocol < 1) |
| `Description` | `string` | Human-readable description |
| `Version` | `string` | Firmware version |
| `Serial` | `string` | Serial number |
| `Location` | `string` | Device path on the OS |
| `ActiveModeIndex` | `int` | Index of the currently active mode |
| `ActiveMode` | `Mode` | Shortcut for `Modes[ActiveModeIndex]` |
| `Modes` | `Mode[]` | All available modes |
| `Zones` | `Zone[]` | Lighting zones on the device |
| `Leds` | `Led[]` | Individual LEDs |
| `Colors` | `Color[]` | Current colors of all LEDs |

---

### `Zone`

| Property | Type | Description |
|---|---|---|
| `Index` | `int` | Zone index on the device |
| `DeviceIndex` | `int` | Parent device index |
| `Name` | `string` | Zone name |
| `Type` | `ZoneType` | `Single`, `Linear`, or `Matrix` |
| `LedCount` | `uint` | Current number of LEDs |
| `LedsMin` | `uint` | Minimum resizable LED count |
| `LedsMax` | `uint` | Maximum resizable LED count |
| `MatrixMap` | `MatrixMap?` | 2D layout (only when `Type == Matrix`) |
| `Segments` | `Segment[]` | Sub-segments (protocol ≥ 4) |

---

### `Mode`

| Property | Type | Description |
|---|---|---|
| `Index` | `int` | Mode index |
| `Name` | `string` | Mode name |
| `Flags` | `ModeFlags` | Capability flags |
| `SpeedMin/Max` | `uint` | Speed range (if `SupportsSpeed`) |
| `BrightnessMin/Max` | `uint` | Brightness range (if `SupportsBrightness`) |
| `ColorMin/Max` | `uint` | Color count range |
| `Speed` | `uint` | Current speed |
| `Brightness` | `uint` | Current brightness |
| `Direction` | `Direction` | Current direction |
| `ColorMode` | `ColorMode` | How colors are applied |
| `Colors` | `Color[]` | Colors used by this mode |
| `SupportsSpeed` | `bool` | Whether speed is configurable |
| `SupportsBrightness` | `bool` | Whether brightness is configurable |
| `SupportsDirection` | `bool` | Whether direction is configurable |

**Mutation methods** (used internally by `UpdateMode`):

```csharp
mode.SetSpeed(uint newSpeed);
mode.SetBrightness(uint newBrightness);
mode.SetDirection(Direction newDirection);
mode.SetColors(Color[] newColors);
```

---

### `Plugin`

Represents an OpenRGB server-side plugin (protocol ≥ 4). Used with `GetPlugins()` and `PluginSpecific()`.

---

## Enumerations

### `DeviceType`

```
Motherboard, Dram, Gpu, Cooler, Ledstrip, Keyboard, Mouse,
Mousemat, Headset, HeadsetStand, Gamepad, Light, Speaker, Virtual, Unknown
```

### `Direction`

```
None, Left, Right, Up, Down, Horizontal, Vertical
```

### `ColorMode`

| Value | Description |
|---|---|
| `None` | Mode uses no color |
| `PerLed` | Each LED has its own color |
| `ModeSpecific` | One color shared by the mode effect |
| `Random` | Colors are randomized by the firmware |

### `ModeFlags` *(bitfield)*

| Flag | Description |
|---|---|
| `HasSpeed` | Mode supports speed parameter |
| `HasDirectionLR` | Left/Right direction |
| `HasDirectionUD` | Up/Down direction |
| `HasDirectionHV` | Horizontal/Vertical direction |
| `HasBrightness` | Brightness parameter |
| `HasPerLedColor` | Per-LED color support |
| `HasModeSpecificColor` | Mode-specific color support |
| `HasRandomColor` | Random color option |

### `ZoneType`

```
Single, Linear, Matrix
```

---

## `ProtocolVersion`

Represents the OpenRGB SDK wire protocol version. The library supports versions 0–4 (OpenRGB up to 0.9).

| Property | Type | Available from | Description |
|---|---|---|---|
| `Number` | `uint` | — | Numeric version |
| `SupportsVendorString` | `bool` | Protocol ≥ 1 | Whether `Device.Vendor` is populated |
| `SupportsProfileControls` | `bool` | Protocol ≥ 2 | `GetProfiles`, `LoadProfile`, `SaveProfile`, `DeleteProfile` |
| `SupportsBrightnessAndSaveMode` | `bool` | Protocol ≥ 3 | `Mode.Brightness` field and `SaveMode()` |
| `SupportsSegmentsAndPlugins` | `bool` | Protocol ≥ 4 | `Zone.Segments`, `GetPlugins()`, `PluginSpecific()` |
