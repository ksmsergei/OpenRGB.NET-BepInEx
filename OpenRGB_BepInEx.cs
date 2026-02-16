using BepInEx;
using BepInEx.Logging;

namespace ksm.OpenRGB;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class OpenRGBBepInEx : BaseUnityPlugin
{
    public static OpenRGBBepInEx Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }
}
