using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace KatamariFix
{
    [BepInPlugin("net.hytus.kamatarifix", "KatamariFix", "1.0.0.0")]
    public class Main : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private void Awake()
        {
            Log = Logger;
            var harmony = new Harmony("net.hytus.harmony.kamatarifix");
            harmony.PatchAll(typeof(AudioPatch));
        }
    }
}
